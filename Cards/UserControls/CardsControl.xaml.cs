using System.Linq;
using System.Windows;

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
                RefreshCards();
            }
        }

        private void RefreshCards()
        {
            _panel.Children.Clear();
            if (_data != null)
                foreach (var card in _data.Cards)
                    if (!HideByFilter(card))
                        _panel.Children.Add(new CardControl {Card = card});
        }

        public CardsControl()
        {
            InitializeComponent();
        }

        private void Filter_OnChecked(object sender, RoutedEventArgs e)
        {
            RefreshCards();
        }

        private bool HideByFilter(Card card)
        {
            if (!card.Available && _cbAvailable.IsChecked == true)
                return true;
            
            return false;
        }
    }
}
