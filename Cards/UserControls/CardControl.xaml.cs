using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
                    _tbBonus1.Visibility = Visibility.Collapsed;
                    _tbBonus2.Visibility = Visibility.Collapsed;
                    _tbCost.Visibility = Visibility.Collapsed;
                    return;
                }

                RefreshData();
            }
        }

        private void RefreshData()
        {
            _tbName.Visibility = Visibility.Visible;
            _tbName.Text = Card.Name;

            var target = Card.TargetId != null ? App.Data.TargetTypes.FirstOrDefault(t => t.Id == Card.TargetId) : null;
            if (target != null)
            {
                _tbTarget.Visibility = Visibility.Visible;
                _tbTarget.Text = "[" + target.Name + "]";
            }
            else
                _tbTarget.Visibility = Visibility.Collapsed;

            if (Card.Bonuses.Length >= 1)
                SetBonusText(_tbBonus1, Card.Bonuses[0]);

            if (Card.Bonuses.Length >= 2)
                SetBonusText(_tbBonus2, Card.Bonuses[1]);

            var source = App.Data.Sources.First(s => s.Id == Card.SourceId);
            _tbCost.Visibility = Visibility.Visible;
            _tbCost.Text = $"{Card.Cost} ({source.Name})";

            if (Card.Available)
                _border.BorderBrush = Brushes.Black;
        }

        private static void SetBonusText(TextBlock textBlock, BonusValue bonus)
        {
            if (bonus == null)
            {
                textBlock.Visibility = Visibility.Collapsed;
                return;
            }

            textBlock.Visibility = Visibility.Visible;
            var bonusType = App.Data.BonusTypes.FirstOrDefault(bt => bt.Id == bonus.BonusTypeId);
            if (bonusType != null)
                textBlock.Text = bonusType.Name + ": " + bonus.Value;
            else
                textBlock.Text = "Error";
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
