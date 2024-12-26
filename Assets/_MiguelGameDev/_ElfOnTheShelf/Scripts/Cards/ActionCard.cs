using UnityEngine;
using UnityEngine.Serialization;

namespace MiguelGameDev.ElfOnTheShelf
{
    [CreateAssetMenu(fileName = "ActionCard", menuName = "ElfOnTheShelf/Action Card")]
    public class ActionCard : Card
    {
        [SerializeField] private CardSuit _suit;
        [SerializeField] private ActionType _actionType;
        
        public override ECardType Type => ECardType.Action;
        public CardSuit Suit => _suit;
        public ActionType ActionType => _actionType;
    }
}