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

        public Guid GradeId { get; set; }
        
        /// <summary>
        /// Имеется в наличии
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Источник получения
        /// </summary>
        public Guid SourceId { get; set; }

        /// <summary>
        /// На какую цель действует карта
        /// </summary>
        public Guid? TargetId { get; set; }

        public BonusValue[] Bonuses { get; set; }

        public Card()
        {
            Bonuses = new BonusValue[0];
        }

        public override string ToString()
        {
            if (App.Data != null)
            {
                var target = TargetId.HasValue ? App.Data.TargetTypes.FirstOrDefault(t => t.Id == TargetId.Value) : null;
                return target != null ? $"{Name} [{target.Name}]" : Name;
            }

            return base.ToString();
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

    public class BonusType : IReferenceRow
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Чем меньше значение, тем лучше
        /// </summary>
        public bool Inverted { get; set; }
    }

    public class TargetType : IReferenceRow
    {
        public const string EmptyName = "-";

        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class BonusValue
    {
        public Guid BonusTypeId { get; set; }
        
        public decimal Value { get; set; }

        public override string ToString()
        {
            if (App.Data != null)
            {
                var bonusType = App.Data.BonusTypes.FirstOrDefault(bt => bt.Id == BonusTypeId);
                if (bonusType != null)
                    return $"{bonusType.Name}: {Value}";
            }
            return base.ToString();
        }
    }

    public interface IReferenceRow
    {
        string Name { get; set; }
    }

    public class Source : IReferenceRow
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class Grade : IReferenceRow
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
