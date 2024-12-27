using MiguelGameDev.Generic.Event;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class BeforeHandDrawHook : IHook
    {
        public string Print()
        {
            return "BeforeHandDraw";
        }
    }
}