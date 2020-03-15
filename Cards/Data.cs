using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Cards
{
    public class Data
    {
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
            var serializer = new JsonSerializer();
            using var writer = new StreamWriter(stream);
            serializer.Serialize(writer, this);
        }

        public static Data Load(Stream stream)
        {
            var serializer = new JsonSerializer();
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);
            return serializer.Deserialize<Data>(jsonReader);
        }

        public void Add(Card card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));

            var list = Cards.ToList();
            list.Add(card);
            Cards = list.ToArray();
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
    }
}
