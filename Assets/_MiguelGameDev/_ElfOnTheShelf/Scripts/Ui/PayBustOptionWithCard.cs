namespace MiguelGameDev.ElfOnTheShelf
{
    public class PayBustOptionWithCard : PayBustOption
    {
        public EPayBustOption Id { get; }
        public CardUi CardUi { get; }
        
        public PayBustOptionWithCard(EPayBustOption id, CardUi cardUi) : base(id)
        {
            CardUi = cardUi;
        }
    }
}