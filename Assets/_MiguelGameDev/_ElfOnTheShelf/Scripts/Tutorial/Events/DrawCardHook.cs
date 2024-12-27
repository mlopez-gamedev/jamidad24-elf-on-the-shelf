using MiguelGameDev.Generic.Event;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class DrawCardHook : IHook
    {
        public HandCardSlot HandCardSlot { get; }

        public DrawCardHook(HandCardSlot handCardSlot)
        {
            HandCardSlot = handCardSlot;
        }
        
        public string Print()
        {
            return "DrawCard";
        }
    }
}