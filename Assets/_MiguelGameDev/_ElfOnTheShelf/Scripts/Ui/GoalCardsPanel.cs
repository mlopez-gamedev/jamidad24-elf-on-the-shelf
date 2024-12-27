using System;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class GoalCardsPanel : MonoBehaviour
    {
        [SerializeField] private Highlight _highlight;
        [SerializeField] private GoalCardSlot[] _goalCardSlots;

        public GoalCardSlot GetEmptyGoalCardSlot(ECardSuit cardSuit)
        {
            foreach (var slot in _goalCardSlots)
            {
                if (slot.CardSuit == cardSuit && slot.IsEmpty)
                {
                    return slot;
                }
            }
            
            throw new ArgumentException($"All goal card suit for {cardSuit} are not empty");
        }
        
        public void PlayHighlight()
        {
            _highlight.Play();
        }
        
        public void StopHighlight()
        {
            _highlight.Stop();
        }

        public void RemoveGoalCard(GoalCardUi cardUi, Transform newParent = null)
        {
            foreach (var goalSlot in _goalCardSlots)
            {
                goalSlot.RemoveCard(cardUi, newParent);
            }
        }
    }
}