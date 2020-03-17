using System;
using System.Collections.Generic;
using System.Linq;

namespace Cards
{
    public interface IOptimalFinder
    {
        IReadOnlyCollection<SearchResult> Search(SearchQuery searchQuery);
    }

    public class SearchQuery
    {
        public int TotalCost { get; }

        public Guid? Target { get; }
        
        public IReadOnlyCollection<Guid> PriorityBonuses { get; }

        public IReadOnlyCollection<Guid> NonPriorityBonuses { get; }

        public SearchQuery(int totalCost, Guid? target, IReadOnlyCollection<Guid> priorityBonuses, IReadOnlyCollection<Guid> nonPriorityBonuses)
        {
            TotalCost = totalCost;
            Target = target;
            PriorityBonuses = priorityBonuses;
            NonPriorityBonuses = nonPriorityBonuses;
        }
    }

    public class SearchResult
    {
        public IReadOnlyCollection<Card> Cards { get; }

        public SearchResult(IReadOnlyCollection<Card> cards)
        {
            Cards = cards;
        }

        public int TotalCost
        {
            get
            {
                return Cards?.Sum(c => c.Cost) ?? 0;
            }
        }

        public IReadOnlyCollection<BonusValue> Bonuses
        {
            get
            {
                var dict = new Dictionary<Guid, decimal>();
                foreach (var cardBonus in Cards.SelectMany(c => c.Bonuses))
                    if (dict.ContainsKey(cardBonus.BonusTypeId))
                        dict[cardBonus.BonusTypeId] += cardBonus.Value;
                    else
                        dict.Add(cardBonus.BonusTypeId, cardBonus.Value);
                return dict
                    .Select(p => new BonusValue { BonusTypeId = p.Key, Value = p.Value })
                    .ToArray();
            }
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Bonuses.Select(b => $"{App.Data.BonusTypes.First(bt => bt.Id == b.BonusTypeId).Name}: {b.Value}"));
        }
    }

    public class OptimalFinder: IOptimalFinder
    {
        private readonly Data _data;

        public OptimalFinder(Data data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public IReadOnlyCollection<SearchResult> Search(SearchQuery searchQuery)
        {
            var cardsBySource = new List<IReadOnlyCollection<Card>>();
            foreach (var source in _data.Sources)
            {
                var cardsFromSource = _data.Cards
                    .Where(c => c.Available)
                    .Where(c => c.TargetId == null || c.TargetId.Value == searchQuery.Target)
                    .Where(c => c.SourceId == source.Id)
                    .Where(c => GetSearchRank(c, searchQuery) > 0)
                    .ToArray();
                if (cardsFromSource.Any())
                    cardsBySource.Add(cardsFromSource);
            }

            var searchResults = GetSearchResults(new List<Card>(),  cardsBySource);

            searchResults = searchResults
                .Where(sr => sr.TotalCost <= searchQuery.TotalCost)
                .ToList();

            if (!searchResults.Any())
                return new SearchResult[0];

            var maxSearchRank = searchResults
                .Max(sr => GetSearchRank(sr, searchQuery));

            return searchResults
                .Where(sr => GetSearchRank(sr, searchQuery) == maxSearchRank)
                .OrderByDescending(sr => sr.Bonuses.Where(b => searchQuery.PriorityBonuses.Contains(b.BonusTypeId)).Sum(b => b.Value))
                .ThenByDescending(sr => sr.Bonuses.Where(b => searchQuery.NonPriorityBonuses.Contains(b.BonusTypeId)).Sum(b => b.Value))
                .ToArray();
        }

        private static int GetSearchRank(Card card, SearchQuery searchQuery)
        {
            var sum = 0;
            foreach (var bonus in card.Bonuses)
            {
                if (searchQuery.PriorityBonuses.Contains(bonus.BonusTypeId))
                    sum += 2;
                if (searchQuery.NonPriorityBonuses.Contains(bonus.BonusTypeId))
                    sum += 1;
            }
            return sum;
        }

        private static int GetSearchRank(SearchResult searchResult, SearchQuery searchQuery)
        {
            var sum = 0;
            foreach (var card in searchResult.Cards)
                sum += GetSearchRank(card, searchQuery);
            return sum;
        }

        private static IEnumerable<SearchResult> GetSearchResults(ICollection<Card> cards, IReadOnlyCollection<IReadOnlyCollection<Card>> collections)
        {
            var remainCollections = collections.Skip(1).ToArray();

            var list = new List<SearchResult>();
            foreach (var card in collections.First())
            {
                var collection = cards.ToList();
                collection.Add(card);

                if (remainCollections.Any())
                    list.AddRange(GetSearchResults(collection, remainCollections));
                else
                    list.Add(new SearchResult(collection));
            }

            return list;
        }
    }
}
