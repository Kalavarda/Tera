using System;
using System.Windows;

namespace Cards.Windows
{
    public partial class ReferenceRowWindow
    {
        private readonly IReferenceRow _referenceRow;

        public ReferenceRowWindow()
        {
            InitializeComponent();
        }

        public ReferenceRowWindow(IReferenceRow referenceRow, string title): this()
        {
            _referenceRow = referenceRow ?? throw new ArgumentNullException(nameof(referenceRow));
            _tbName.Text = _referenceRow.Name;
            Title = title;
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _referenceRow.Name = _tbName.Text;
                DialogResult = true;
            }
            catch (Exception error)
            {
                App.ShowError(error);
            }
        }
    }
}
