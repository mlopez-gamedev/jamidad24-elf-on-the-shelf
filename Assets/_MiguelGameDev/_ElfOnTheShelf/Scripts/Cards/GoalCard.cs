using UnityEngine;
using UnityEngine.Serialization;

namespace MiguelGameDev.ElfOnTheShelf
{
    [CreateAssetMenu(fileName = "ObjectiveCard", menuName = "ElfOnTheShelf/Objective Card")]
    public class GoalCard : Card
    {
        [FormerlySerializedAs("_color")] [SerializeField] private CardSuit _suit;
        
        public override ECardType Type => ECardType.Goal;
        public CardSuit Suit => _suit;
    }
}