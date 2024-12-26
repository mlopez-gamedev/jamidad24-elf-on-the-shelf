using Sirenix.OdinInspector;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class GoalCardSlot : MonoBehaviour
    {
        [SerializeField] private ECardSuit _cardSuit;
        
        [ShowInInspector, HideInEditorMode] private GoalCardUi _goalCardUi;
        
        public ECardSuit CardSuit => _cardSuit;
        public bool IsEmpty => _goalCardUi == null;

        public void AddCard(GoalCardUi cardUi)
        {
            _goalCardUi = cardUi;
            _goalCardUi.transform.SetParent(transform);
        }
        
        public void RemoveCard(GoalCardUi cardUi, Transform newParent = null)
        {
            if (_goalCardUi != cardUi)
            {
                return;
            }
            _goalCardUi.transform.SetParent(newParent);
            _goalCardUi = null;
        }
    }
}