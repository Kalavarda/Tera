using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cards.Windows
{
    public partial class TargetTypesWindow
    {
        private readonly Data _data;
        
        private IReadOnlyCollection<TargetType> SelectedTargetTypes => _lb.SelectedItems.OfType<TargetType>().ToArray();

        private TargetType SelectedTargetType
        {
            get
            {
                var selected = SelectedTargetTypes;
                return selected.Count == 1 ? selected.First() : null;
            }
        }

        public TargetTypesWindow()
        {
            InitializeComponent();
        }

        public TargetTypesWindow(Data data): this()
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            TuneControls();
        }

        private void TuneControls()
        {
            var selected = SelectedTargetTypes.Select(c => c.Id).ToArray();
            _lb.ItemsSource = _data.TargetTypes.OrderBy(c => c.Name);
            if (selected.Any())
                _lb.SelectedItem = _data.TargetTypes.FirstOrDefault(c => selected.Any(s => s == c.Id));

            _btnEdit.IsEnabled = SelectedTargetType != null;
            _btnRemove.IsEnabled = SelectedTargetTypes.Any();
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            var source = new TargetType { Id = Guid.NewGuid() };
            if (new ReferenceRowWindow(source, "Цель") { Owner = this }.ShowDialog() == true)
                _data.Add(source);

            TuneControls();

            _lb.SelectedItem = source;
        }

        private void OnRemoveClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) != MessageBoxResult.Yes)
                return;

            _data.Remove(SelectedTargetTypes);
            TuneControls();
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (new ReferenceRowWindow(SelectedTargetType, "Цель") { Owner = this }.ShowDialog() == true)
                TuneControls();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TuneControls();
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OnEditClick(sender, null);
        }
    }
}
