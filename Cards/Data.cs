﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Cards
{
    public class Data
    {
        public static IReadOnlyCollection<int> Costs = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public Card[] Cards { get; set; }

        public BonusType[] BonusTypes { get; set; }

        public Source[] Sources { get; set; }

        public Grade[] Grades { get; set; }

        public TargetType[] TargetTypes { get; set; }

        public Data()
        {
            Cards = new Card[0];
            BonusTypes = new BonusType[0];
            Sources = new Source[0];
            Grades = new[]
            {
                new Grade { Id = Guid.NewGuid(), Name = "Серый" },
                new Grade { Id = Guid.NewGuid(), Name = "Зелёный" },
            };
            TargetTypes = new TargetType[0];
        }

        public void Save(Stream stream)
        {
            CalculateBonusInvert();

            var serializer = new JsonSerializer();
            using var writer = new StreamWriter(stream);
            serializer.Serialize(writer, this);
        }

        public static Data Load(Stream stream)
        {
            var serializer = new JsonSerializer { Formatting = Formatting.Indented };
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);
            var data = serializer.Deserialize<Data>(jsonReader);
            data.CalculateBonusInvert();
            return data;
        }

        public void Add(Card card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));

            var list = Cards.ToList();
            list.Add(card);
            Cards = list.ToArray();

            CalculateBonusInvert();
        }

        public void Add(BonusType bonusType)
        {
            if (bonusType == null) throw new ArgumentNullException(nameof(bonusType));

            var list = BonusTypes.ToList();
            list.Add(bonusType);
            BonusTypes = list.ToArray();
        }

        public void Add(Source source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var list = Sources.ToList();
            list.Add(source);
            Sources = list.ToArray();
        }

        public void Add(Grade grade)
        {
            if (grade == null) throw new ArgumentNullException(nameof(grade));

            var list = Grades.ToList();
            list.Add(grade);
            Grades = list.ToArray();
        }

        public void Add(TargetType targetType)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            var list = TargetTypes.ToList();
            list.Add(targetType);
            TargetTypes = list.ToArray();
        }

        public void Remove(IReadOnlyCollection<Card> cards)
        {
            if (cards == null) throw new ArgumentNullException(nameof(cards));

            var list = Cards.ToList();
            list.RemoveAll(cards.Contains);
            Cards = list.ToArray();
        }

        public void Remove(IReadOnlyCollection<BonusType> bonusTypes)
        {
            if (bonusTypes == null) throw new ArgumentNullException(nameof(bonusTypes));

            var list = BonusTypes.ToList();
            list.RemoveAll(bonusTypes.Contains);
            BonusTypes = list.ToArray();
        }

        public void Remove(IReadOnlyCollection<Source> sources)
        {
            if (sources == null) throw new ArgumentNullException(nameof(sources));

            var list = Sources.ToList();
            list.RemoveAll(sources.Contains);
            Sources = list.ToArray();
        }

        public void Remove(IReadOnlyCollection<Grade> grades)
        {
            if (grades == null) throw new ArgumentNullException(nameof(grades));

            var list = Grades.ToList();
            list.RemoveAll(grades.Contains);
            Grades = list.ToArray();
        }

        public void Remove(IReadOnlyCollection<TargetType> targetTypes)
        {
            if (targetTypes == null) throw new ArgumentNullException(nameof(targetTypes));

            var list = TargetTypes.ToList();
            list.RemoveAll(targetTypes.Contains);
            TargetTypes = list.ToArray();
        }

        private void CalculateBonusInvert()
        {
            var dict = new Dictionary<Guid, decimal>();
            foreach (var bonus in Cards.SelectMany(c => c.Bonuses))
                if (dict.ContainsKey(bonus.BonusTypeId))
                    dict[bonus.BonusTypeId] += bonus.Value;
                else
                    dict.Add(bonus.BonusTypeId, bonus.Value);
            
            foreach (var pair in dict)
            {
                var bonusType = BonusTypes.FirstOrDefault(bt => bt.Id == pair.Key);
                if (bonusType != null)
                    bonusType.Inverted = pair.Value < 0;
            }
        }
    }
}
