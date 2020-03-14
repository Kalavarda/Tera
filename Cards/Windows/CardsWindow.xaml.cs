using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cards.Windows
{
    public partial class CardsWindow
    {
        private readonly Data _data;

        private IReadOnlyCollection<Card> SelectedCards => _lb.SelectedItems.OfType<Card>().ToArray();

        private Card SelectedCard
        {
            get
            {
                var selectedCards = SelectedCards;
                return selectedCards.Count == 1 ? selectedCards.First() : null;
            }
        }

        public CardsWindow()
        {
            InitializeComponent();
        }

        public CardsWindow(Data data): this()
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            TuneControls();
        }

        private void TuneControls()
        {
            var selected = SelectedCards.Select(c => c.Id).ToArray();
            _lb.ItemsSource = _data.Cards.OrderBy(c => c.Name);
            if (selected.Any())
                _lb.SelectedItem = _data.Cards.FirstOrDefault(c => selected.Any(s => s == c.Id));

            _btnEdit.IsEnabled = SelectedCard != null;
            _btnRemove.IsEnabled = SelectedCards.Any();
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            var card = new Card { Id = Guid.NewGuid() };
            if (new CardWindow(card) {Owner = this}.ShowDialog() == true)
                _data.Add(card);

            TuneControls();

            _lb.SelectedItem = card;
        }

        private void OnRemoveClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) != MessageBoxResult.Yes)
                return;

            _data.Remove(SelectedCards);
            TuneControls();
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (new CardWindow(SelectedCard) { Owner = this }.ShowDialog() == true)
                TuneControls();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TuneControls();
        }
    }
}
