using System;
using System.Windows;

namespace Cards.Windows
{
    public partial class CardWindow
    {
        private readonly Card _card;

        public CardWindow()
        {
            InitializeComponent();

            _cbCost.ItemsSource = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            _cbSource.ItemsSource = Enum.GetValues(typeof(Source));
            _cbQuality.ItemsSource = Enum.GetValues(typeof(Quality));
        }

        public CardWindow(Card card): this()
        {
            _card = card ?? throw new ArgumentNullException(nameof(card));

            _tbName.Text = _card.Name;
            _cbAvailable.IsChecked = _card.Available;
            _cbCost.SelectedItem = _card.Cost;
            _cbSource.SelectedItem = _card.Source;
            _cbQuality.SelectedItem = _card.Quality;
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _card.Name = _tbName.Text;
                _card.Available = _cbAvailable.IsChecked == true;
                _card.Cost = (int)_cbCost.SelectedItem;
                _card.Source = (Source)_cbSource.SelectedItem;
                _card.Quality = (Quality)_cbQuality.SelectedItem;

                DialogResult = true;
            }
            catch (Exception error)
            {
                App.ShowError(error);
            }
        }
    }
}
