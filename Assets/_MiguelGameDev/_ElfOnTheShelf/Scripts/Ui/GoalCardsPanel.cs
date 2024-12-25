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
        
        public void SetHighlight(bool enable)
        {
            if (enable)
            {
                _highlight.Play();
            }
            else
            {
                _highlight.Stop();
            }
        }
    }
}