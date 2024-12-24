using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    [CreateAssetMenu(fileName = "BustCard", menuName = "ElfOnTheShelf/Bust Card")]
    public class BustCard : Card
    {
        public override ECardType Type => ECardType.Bust;
    }
}