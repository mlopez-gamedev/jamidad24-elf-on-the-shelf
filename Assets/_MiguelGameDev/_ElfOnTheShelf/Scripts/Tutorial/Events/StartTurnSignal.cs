using MiguelGameDev.Generic.Event;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class StartTurnSignal : ISignal
    {
        public string Print()
        {
            return "StartTurn";
        }
    }
}