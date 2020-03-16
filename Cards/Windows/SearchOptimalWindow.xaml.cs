using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Cards.Windows
{
    public partial class SearchOptimalWindow
    {
        private readonly Data _data;

        public SearchOptimalWindow()
        {
            InitializeComponent();
        }

        public SearchOptimalWindow(Data data): this()
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));

            _cbTarget.ItemsSource = data.TargetTypes.OrderBy(bt => bt.Name);

            _icBonuses1.ItemsSource = _data.BonusTypes
                .OrderBy(b => b.Name)
                .Select(b => new CheckBox { DataContext = b, Content = b.Name });
            _icBonuses2.ItemsSource = _data.BonusTypes
                .OrderBy(b => b.Name)
                .Select(b => new CheckBox { DataContext = b, Content = b.Name });
        }

        private void OnSearchClick(object sender, RoutedEventArgs e)
        {
            var totalCost = int.Parse(_tbCost.Text);

            var targetId = CardWindow.GetSelectedTarget((TargetType)_cbTarget.SelectedItem);

            var bonusIds1 = new List<Guid>();
            foreach (var checkBox in _icBonuses1.Items.Cast<ToggleButton>())
                if (checkBox.IsChecked == true)
                {
                    var bonus = (BonusType)checkBox.DataContext;
                    bonusIds1.Add(bonus.Id);
                }

            var bonusIds2 = new List<Guid>();
            foreach (var checkBox in _icBonuses1.Items.Cast<ToggleButton>())
                if (checkBox.IsChecked == true)
                {
                    var bonus = (BonusType)checkBox.DataContext;
                    bonusIds2.Add(bonus.Id);
                }

            IOptimalFinder finder = new OptimalFinder();
            var cards = finder.Find(totalCost, targetId, bonusIds1, bonusIds2);

            throw new NotImplementedException();
        }
    }
}
