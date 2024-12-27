using MiguelGameDev.Generic.Event;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class CardSelectedSignal : ISignal
    {
        public CardUi CardUi { get; }

        public CardSelectedSignal(CardUi cardUi)
        {
            CardUi = cardUi;
        }
        
        public string Print()
        {
            return "CardSelected";
        }
    }
}