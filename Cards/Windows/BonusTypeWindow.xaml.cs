using System;
using System.Windows;

namespace Cards.Windows
{
    public partial class BonusTypeWindow
    {
        private readonly BonusType _bonusType;

        public BonusTypeWindow()
        {
            InitializeComponent();
        }

        public BonusTypeWindow(BonusType bonusType): this()
        {
            _bonusType = bonusType ?? throw new ArgumentNullException(nameof(bonusType));
            _tbName.Text = _bonusType.Name;
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _bonusType.Name = _tbName.Text;
                DialogResult = true;
            }
            catch (Exception error)
            {
                App.ShowError(error);
            }
        }
    }
}
