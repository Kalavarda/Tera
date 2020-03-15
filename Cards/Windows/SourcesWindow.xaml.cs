using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cards.Windows
{
    public partial class SourcesWindow
    {
        private readonly Data _data;

        private IReadOnlyCollection<Source> SelectedSources => _lb.SelectedItems.OfType<Source>().ToArray();

        private Source SelectedSource
        {
            get
            {
                var selected = SelectedSources;
                return selected.Count == 1 ? selected.First() : null;
            }
        }

        public SourcesWindow()
        {
            InitializeComponent();
        }

        public SourcesWindow(Data data): this()
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            TuneControls();
        }

        private void TuneControls()
        {
            var selected = SelectedSources.Select(c => c.Id).ToArray();
            _lb.ItemsSource = _data.Sources.OrderBy(c => c.Name);
            if (selected.Any())
                _lb.SelectedItem = _data.Sources.FirstOrDefault(c => selected.Any(s => s == c.Id));

            _btnEdit.IsEnabled = SelectedSource != null;
            _btnRemove.IsEnabled = SelectedSources.Any();
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            var source = new Source { Id = Guid.NewGuid() };
            if (new ReferenceRowWindow(source, "Источник получения") { Owner = this }.ShowDialog() == true)
                _data.Add(source);

            TuneControls();

            _lb.SelectedItem = source;
        }

        private void OnRemoveClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) != MessageBoxResult.Yes)
                return;

            _data.Remove(SelectedSources);
            TuneControls();
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (new ReferenceRowWindow(SelectedSource, "Источник получения") { Owner = this }.ShowDialog() == true)
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
