using MiguelGameDev.Generic.Event;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class SendCardToMagicalPortalHook : IHook
    {
        public CardUi CardUi { get; }

        public SendCardToMagicalPortalHook(CardUi cardUi)
        {
            CardUi = cardUi;
        }
        
        public string Print()
        {
            return "DrawCard";
        }
    }
}