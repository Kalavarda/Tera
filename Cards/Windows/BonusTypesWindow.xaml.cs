using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cards.Windows
{
    public partial class BonusTypesWindow
    {
        private readonly Data _data;

        private IReadOnlyCollection<BonusType> SelectedBonusTypes => _lb.SelectedItems.OfType<BonusType>().ToArray();

        private BonusType SelectedBonusType
        {
            get
            {
                var selected = SelectedBonusTypes;
                return selected.Count == 1 ? selected.First() : null;
            }
        }

        public BonusTypesWindow()
        {
            InitializeComponent();
        }

        public BonusTypesWindow(Data data): this()
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            TuneControls();
        }

        private void TuneControls()
        {
            var selected = SelectedBonusTypes.Select(c => c.Id).ToArray();
            _lb.ItemsSource = _data.BonusTypes.OrderBy(c => c.Name);
            if (selected.Any())
                _lb.SelectedItem = _data.BonusTypes.FirstOrDefault(c => selected.Any(s => s == c.Id));

            _btnEdit.IsEnabled = SelectedBonusType != null;
            _btnRemove.IsEnabled = SelectedBonusTypes.Any();
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            var bonusType = new BonusType { Id = Guid.NewGuid() };
            if (new BonusTypeWindow(bonusType) { Owner = this }.ShowDialog() == true)
                _data.Add(bonusType);

            TuneControls();

            _lb.SelectedItem = bonusType;
        }

        private void OnRemoveClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) != MessageBoxResult.Yes)
                return;

            _data.Remove(SelectedBonusTypes);
            TuneControls();
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (new BonusTypeWindow(SelectedBonusType) { Owner = this }.ShowDialog() == true)
                TuneControls();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TuneControls();
        }
    }
}
