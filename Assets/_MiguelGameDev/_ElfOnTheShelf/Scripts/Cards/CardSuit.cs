using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    [CreateAssetMenu(fileName = "CardSuit", menuName = "ElfOnTheShelf/Card Suit")]
    public class CardSuit : ScriptableObject
    {
        [SerializeField] private ECardSuit _id;
        [SerializeField] private Color _backgroundTopColor = Color.white;
        [SerializeField] private Color _backgroundBottomColor = Color.white;
        [SerializeField] private Color _strokeColor = Color.white;

        public ECardSuit Id => _id;
        public Color BackgroundTopColor => _backgroundTopColor;
        public Color BackgroundBottomColor => _backgroundBottomColor;
        public Color StrokeColor => _strokeColor;
    }
}