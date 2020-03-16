using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Cards.Windows;

namespace Cards.UserControls
{
    public partial class CardsControl
    {
        private Data _data;

        public Data Data
        {
            get => _data;
            set
            {
                _data = value;

                _cbTarget.ItemsSource = _data.TargetTypes.OrderBy(t => t.Name);

                _icGrades.ItemsSource = _data.Grades
                    .OrderBy(g => g.Name)
                    .Select(g =>
                    {
                        var checkBox = new CheckBox { DataContext = g, Content = g.Name };
                        checkBox.Checked += Filter_OnChanged;
                        checkBox.Unchecked += Filter_OnChanged;
                        return checkBox;
                    });

                _icBonuses.ItemsSource = _data.BonusTypes
                    .OrderBy(b => b.Name)
                    .Select(b =>
                    {
                        var checkBox = new CheckBox { DataContext = b, Content = b.Name };
                        checkBox.Checked += Filter_OnChanged;
                        checkBox.Unchecked += Filter_OnChanged;
                        return checkBox;
                    });

                RefreshCards();
            }
        }

        private void RefreshCards()
        {
            _panel.Children.Clear();
            if (_data != null)
                foreach (var card in _data.Cards)
                    if (!HideByFilter(card))
                        _panel.Children.Add(new CardControl { Card = card });
        }

        public CardsControl()
        {
            InitializeComponent();
        }

        private void Filter_OnChanged(object sender, RoutedEventArgs e)
        {
            RefreshCards();
        }

        private bool HideByFilter(Card card)
        {
            var filter = GetFilter();

            if (!card.Available && filter.Available)
                return true;

            if (filter.GradeIds.Any())
                if (!filter.GradeIds.Contains(card.GradeId))
                    return true;

            if (filter.BonusIds.Any())
                if (!filter.BonusIds.Any(b => card.Bonuses.Any(bv => bv.BonusTypeId == b)))
                    return true;

            if (filter.TargetId != null)
                if (card.TargetId != null && card.TargetId.Value != filter.TargetId.Value)
                    return true;

            return false;
        }

        private Filter GetFilter()
        {
            var targetId = CardWindow.GetSelectedTarget(_cbTarget.SelectedItem as TargetType);

            var gradeIds = new List<Guid>();
            foreach (var checkBox in _icGrades.Items.Cast<ToggleButton>())
                if (checkBox.IsChecked == true)
                {
                    var grade = (Grade)checkBox.DataContext;
                    gradeIds.Add(grade.Id);
                }

            var bonusIds = new List<Guid>();
            foreach (var checkBox in _icBonuses.Items.Cast<ToggleButton>())
                if (checkBox.IsChecked == true)
                {
                    var bonus = (BonusType)checkBox.DataContext;
                    bonusIds.Add(bonus.Id);
                }

            return new Filter
            {
                Available = _cbAvailable.IsChecked == true,
                TargetId = targetId,
                GradeIds = gradeIds,
                BonusIds = bonusIds
            };
        }

        public class Filter
        {
            public bool Available { get; set; }

            public Guid? TargetId { get; set; }

            public IReadOnlyCollection<Guid> GradeIds { get; set; }

            public IReadOnlyCollection<Guid> BonusIds { get; set; }
        }
    }
}
