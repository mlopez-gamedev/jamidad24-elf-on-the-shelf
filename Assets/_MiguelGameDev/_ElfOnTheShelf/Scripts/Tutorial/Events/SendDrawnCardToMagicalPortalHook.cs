using MiguelGameDev.Generic.Event;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class SendDrawnCardToMagicalPortalHook : IHook
    {
        public CardUi CardUi { get; }

        public SendDrawnCardToMagicalPortalHook(CardUi cardUi)
        {
            CardUi = cardUi;
        }
        
        public string Print()
        {
            return "DrawCard";
        }
    }
}