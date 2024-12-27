namespace MiguelGameDev.ElfOnTheShelf
{
    public struct CardsAmount {
        public int  Pranks { get; }
        public int  Goodies { get; }
        public int  Tricks { get; }
        
        public CardsAmount(int pranks, int goodies, int tricks)
        {
            Pranks = pranks;
            Goodies = goodies;
            Tricks = tricks;
        }
    }
}