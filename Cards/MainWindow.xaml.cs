using System.IO;
using System.Windows;
using Cards.Windows;
using Microsoft.Win32;

namespace Cards
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Load(string fileName)
        {
            using var file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            App.Load(file);
            //App.CurrentCharacter = _data.Characters.FirstOrDefault();
            //TuneControls(_data);
            _cardsControl.Data = App.Data;
        }

        private void OnLoadClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            TuneFileDialog(fileDialog);
            if (fileDialog.ShowDialog() == true)
                Load(fileDialog.FileName);
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new SaveFileDialog();
            TuneFileDialog(fileDialog);
            if (fileDialog.ShowDialog() == true)
            {
                using (var file = new FileStream(fileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    App.Data.Save(file);

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
            App.New();
            _cardsControl.Data = App.Data;
        }

        private void OnCardsClick(object sender, RoutedEventArgs e)
        {
            new CardsWindow(App.Data) { Owner = this }.ShowDialog();
        }

        private void OnBonusTypesClick(object sender, RoutedEventArgs e)
        {
            new BonusTypesWindow(App.Data) { Owner = this}.ShowDialog();
        }

        private void OnSourcesClick(object sender, RoutedEventArgs e)
        {
            new SourcesWindow(App.Data) { Owner = this }.ShowDialog();
        }

        private void OnTargetTypesClick(object sender, RoutedEventArgs e)
        {
            new TargetTypesWindow(App.Data) { Owner = this }.ShowDialog();
        }

        private void OnSearchOptimalClick(object sender, RoutedEventArgs e)
        {
            new SearchOptimalWindow(App.Data) { Owner = this }.ShowDialog();
        }
    }
}
