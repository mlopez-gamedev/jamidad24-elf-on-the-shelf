using MiguelGameDev.Generic.Event;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class ShuffleDeckSignal : IHook
    {
        public string Print()
        {
            return "ShuffleDeck";
        }
    }
}