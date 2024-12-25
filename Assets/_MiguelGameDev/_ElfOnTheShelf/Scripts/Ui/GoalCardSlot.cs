using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class GoalCardSlot : MonoBehaviour
    {
        [SerializeField] private ECardSuit _cardSuit;
        
        private GoalCardUi _goalCardUi;
        
        public ECardSuit CardSuit => _cardSuit;
        public bool IsEmpty => _goalCardUi == null;

        public void AddCard(GoalCardUi cardUi)
        {
            _goalCardUi = cardUi;
            _goalCardUi.transform.SetParent(transform);
        }
    }
}