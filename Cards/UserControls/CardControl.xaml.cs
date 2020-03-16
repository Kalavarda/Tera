namespace Cards.UserControls
{
    public partial class CardControl
    {
        public Card Card
        {
            get => DataContext as Card;
            set => DataContext = value;
        }

        public CardControl()
        {
            InitializeComponent();
        }
    }
}
