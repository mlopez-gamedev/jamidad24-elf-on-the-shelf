using I2.Loc;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public abstract class Card : ScriptableObject
    {
        [SerializeField] private Sprite _picture;

        public abstract ECardType Type { get; }
        public Sprite Picture => _picture;
    }
}