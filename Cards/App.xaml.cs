using System;
using System.Windows;

namespace Cards
{
    public partial class App
    {
        public static void ShowError(Exception error)
        {
            MessageBox.Show(error.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
