using MiguelGameDev.Generic.Event;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class StartGameHook : IHook
    {
        public string Print()
        {
            return "StartGame";
        }
    }
}