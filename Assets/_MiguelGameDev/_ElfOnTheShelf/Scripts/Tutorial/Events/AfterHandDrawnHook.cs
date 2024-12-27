using MiguelGameDev.Generic.Event;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class AfterHandDrawnHook : IHook
    {
        public string Print()
        {
            return "AfterHandDrawn";
        }
    }
}