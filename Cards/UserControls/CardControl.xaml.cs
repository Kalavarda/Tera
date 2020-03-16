using System.Linq;
using System.Windows;
using System.Windows.Input;
using Cards.Windows;

namespace Cards.UserControls
{
    public partial class CardControl
    {
        public Card Card
        {
            get => DataContext as Card;
            set
            {
                DataContext = value;

                if (value == null)
                {
                    _tbName.Visibility = Visibility.Collapsed;
                    _tbTarget.Visibility = Visibility.Collapsed;
                    return;
                }

                RefreshData();
            }
        }

        private void RefreshData()
        {
            _tbName.Text = Card.Name;

            var target = Card.TargetId != null ? App.Data.TargetTypes.FirstOrDefault(t => t.Id == Card.TargetId) : null;
            if (target != null)
            {
                _tbTarget.Visibility = Visibility.Visible;
                _tbTarget.Text = target.Name;
            }
            else
                _tbTarget.Visibility = Visibility.Collapsed;
        }

        public CardControl()
        {
            InitializeComponent();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (new CardWindow(Card, App.Data) { Owner = App.GetWindow(this) }.ShowDialog() == true)
                RefreshData();
        }
    }
}
