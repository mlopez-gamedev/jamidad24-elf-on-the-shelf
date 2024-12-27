using MiguelGameDev.Generic.Event;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class CardDiscardedSignal : ISignal
    {
        public string Print()
        {
            return "CardDiscarded";
        }
    }
}