using System.IO;
using System.Windows;
using Cards.Windows;
using Microsoft.Win32;

namespace Cards
{
    public partial class MainWindow
    {
        private Data _data = new Data();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Load(string fileName)
        {
            using var file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            _data = Data.Load(file);
            //App.CurrentCharacter = _data.Characters.FirstOrDefault();
            //TuneControls(_data);
        }

        private void OnLoadClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            TuneFileDialog(fileDialog);
            if (fileDialog.ShowDialog() == true)
            {
                Load(fileDialog.FileName);
            }
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new SaveFileDialog();
            TuneFileDialog(fileDialog);
            if (fileDialog.ShowDialog() == true)
            {
                using (var file = new FileStream(fileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    _data.Save(file);

                //Settings.Default.LastFileName = fileDialog.FileName;
                //Settings.Default.Save();
            }
        }
        
        private static void TuneFileDialog(FileDialog fileDialog)
        {
            fileDialog.DefaultExt = ".cards.json";
            fileDialog.Filter = "Карты|*.cards.json|Все файлы|*.*";
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnNewClick(object sender, RoutedEventArgs e)
        {
            _data = new Data();
        }

        private void OnCardsClick(object sender, RoutedEventArgs e)
        {
            new CardsWindow(_data) { Owner = this }.Show();
        }

        private void OnBonusTypesClick(object sender, RoutedEventArgs e)
        {
            new BonusTypesWindow(_data) { Owner = this }.Show();
        }
    }
}
