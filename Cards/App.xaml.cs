using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Cards
{
    public partial class App
    {
        public static Data Data { get; private set; }

        public static void New()
        {
            Data = new Data();
        }

        public static void Load(Stream stream)
        {
            Data = Data.Load(stream);
        }

        public static void ShowError(Exception error)
        {
            MessageBox.Show(error.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static Window GetWindow(FrameworkElement userControl)
        {
            if (userControl == null) throw new ArgumentNullException(nameof(userControl));

            var w2 = userControl as Window;
            if (w2 != null)
                return w2;

            var uc = userControl;
            var w = uc.Parent as Window;

            while (w == null)
            {
                uc = (FrameworkElement)uc.Parent;

                if (uc == null)
                    return App.Current.MainWindow;

                w = uc.Parent as Window;
            }

            return w;
        }
    }
}
