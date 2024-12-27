using MiguelGameDev.Generic.Event;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class CardPlayedSignal : ISignal
    {
        public string Print()
        {
            return "CardPlayed";
        }
    }
}