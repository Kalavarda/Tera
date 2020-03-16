using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Cards.UserControls;

namespace Cards.Windows
{
    public partial class CardsWindow
    {
        private readonly Data _data;

        private IReadOnlyCollection<Card> SelectedCards => _lb.SelectedItems.OfType<CardControl>().Select(c => c.Card).ToArray();

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
            var cardControls = _data.Cards.OrderBy(c => c.Name).Select(c => new CardControl { Card = c });
            _lb.ItemsSource = cardControls;
            if (selected.Any())
            {
                var selectedCard = _data.Cards.FirstOrDefault(c => selected.Any(s => s == c.Id));
                _lb.SelectedItem = cardControls.FirstOrDefault(cc => cc.Card == selectedCard);
            }

            _btnEdit.IsEnabled = SelectedCard != null;
            _btnRemove.IsEnabled = SelectedCards.Any();
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            var card = new Card { Id = Guid.NewGuid() };
            if (new CardWindow(card, _data) {Owner = this}.ShowDialog() == true)
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
            if (new CardWindow(SelectedCard, _data) { Owner = this }.ShowDialog() == true)
                TuneControls();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TuneControls();
        }
    }
}
