using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Cards
{
    public class Card
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Cost { get; set; }

        public Quality Quality { get; set; }
        
        /// <summary>
        /// Имеется в наличии
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Источник получения
        /// </summary>
        public Source Source { get; set; }

        public BonusValue[] Bonuses { get; set; }

        public Card()
        {
            Bonuses = new BonusValue[0];
        }
    }

    public enum Quality
    {
        /// <summary>
        /// Обычный серый
        /// </summary>
        Common,

        /// <summary>
        /// Редкий зелёный
        /// </summary>
        Rare
    }

    public enum Source
    {
        /// <summary>
        /// Персонажи
        /// </summary>
        Character,

        /// <summary>
        /// Монстры
        /// </summary>
        Monsters,

        /// <summary>
        /// Материалы
        /// </summary>
        Materials,

        /// <summary>
        /// Рыбалка
        /// </summary>
        Fishing,

        /// <summary>
        /// Территории
        /// </summary>
        Territories
    }

    public class BonusType
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class BonusValue
    {
        public Guid BonusTypeId { get; set; }
        
        public decimal Value { get; set; }
    }

    public class Data
    {
        public Card[] Cards { get; set; }

        public BonusType[] BonusTypes { get; set; }

        public Data()
        {
            Cards = new Card[0];
            BonusTypes = new BonusType[0];
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
    }
}
