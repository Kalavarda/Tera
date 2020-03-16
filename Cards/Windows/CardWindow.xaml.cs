using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Cards.Windows
{
    public partial class CardWindow
    {
        private readonly Card _card;

        public CardWindow()
        {
            InitializeComponent();

            _cbCost.ItemsSource = Data.Costs;
            _cbQuality.ItemsSource = Enum.GetValues(typeof(Quality));
        }

        public CardWindow(Card card, Data data): this()
        {
            _card = card ?? throw new ArgumentNullException(nameof(card));

            _cbSource.ItemsSource = data.Sources.OrderBy(bt => bt.Name);
            _cbQuality.ItemsSource = data.Grades.OrderBy(bt => bt.Name);
            _cbTarget.ItemsSource = data.TargetTypes.OrderBy(bt => bt.Name);
            _cbBonus1Type.ItemsSource = data.BonusTypes.OrderBy(bt => bt.Name);
            _cbBonus2Type.ItemsSource = data.BonusTypes.OrderBy(bt => bt.Name);

            _tbName.Text = _card.Name;
            _cbAvailable.IsChecked = _card.Available;
            _cbCost.SelectedItem = _card.Cost;
            _cbSource.SelectedItem = data.Sources.FirstOrDefault(s => s.Id == _card.SourceId);
            _cbQuality.SelectedItem = data.Grades.FirstOrDefault(s => s.Id == _card.GradeId);
            _cbTarget.SelectedItem = data.TargetTypes.FirstOrDefault(s => s.Id == _card.TargetId);

            var bonus1 = _card.Bonuses.Length >= 1 ? _card.Bonuses[0] : null;
            if (bonus1 != null)
            {
                _cbBonus1Type.SelectedItem = data.BonusTypes.FirstOrDefault(bt => bt.Id == bonus1.BonusTypeId);
                _tbBonus1Value.Text = bonus1.Value.ToString();
            }

            var bonus2 = _card.Bonuses.Length >= 2 ? _card.Bonuses[1] : null;
            if (bonus2 != null)
            {
                _cbBonus2Type.SelectedItem = data.BonusTypes.FirstOrDefault(bt => bt.Id == bonus2.BonusTypeId);
                _tbBonus2Value.Text = bonus2.Value.ToString();
            }
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _card.Name = _tbName.Text;
                _card.Available = _cbAvailable.IsChecked == true;
                _card.Cost = (int)_cbCost.SelectedItem;
                _card.SourceId = ((Source)_cbSource.SelectedItem).Id;
                _card.GradeId = ((Grade)_cbQuality.SelectedItem).Id;
                _card.Bonuses = ParseBonuses();
                _card.TargetId = GetSelectedTarget((TargetType)_cbTarget.SelectedItem);

                DialogResult = true;
            }
            catch (Exception error)
            {
                App.ShowError(error);
            }
        }

        public static Guid? GetSelectedTarget(TargetType selectedItem)
        {
            if (selectedItem == null)
                return null;
            return selectedItem.Name != TargetType.EmptyName ? selectedItem.Id : (Guid?)null;
        }

        private BonusValue[] ParseBonuses()
        {
            BonusValue bonus1 = null;
            if (_cbBonus1Type.SelectedItem is BonusType bonus1Type)
                bonus1 = new BonusValue
                {
                    BonusTypeId = bonus1Type.Id,
                    Value = decimal.Parse(_tbBonus1Value.Text)
                };

            BonusValue bonus2 = null;
            if (_cbBonus2Type.SelectedItem is BonusType bonus2Type)
                bonus2 = new BonusValue
                {
                    BonusTypeId = bonus2Type.Id,
                    Value = decimal.Parse(_tbBonus2Value.Text)
                };

            var bonusValues = new List<BonusValue>();
            if (bonus1 != null)
                bonusValues.Add(bonus1);
            if (bonus2 != null)
                bonusValues.Add(bonus2);
            return bonusValues.ToArray();
        }
    }
}
