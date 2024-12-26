using System;
using System.Linq;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class HandCards : MonoBehaviour
    {
        [SerializeField] private HandCardSlot[] _cardSlots;
        
        public bool TryGetFirstEmptySlot(out HandCardSlot filledCardSlot)
        {
            foreach (var cardSlot in _cardSlots)
            {
                if (!cardSlot.IsEmpty)
                {
                    continue;
                }
                
                filledCardSlot = cardSlot;
                return true;
            }
            
            filledCardSlot = null;
            return false;
        }
        
        public bool TryGetLastNotEmptySlot(out HandCardSlot filledCardSlot)
        {
            var reversedCards = _cardSlots.Reverse();
            foreach (var cardSlot in reversedCards)
            {
                if (cardSlot.IsEmpty)
                {
                    continue;
                }
                
                filledCardSlot = cardSlot;
                return true;
            }
            
            filledCardSlot = null;
            return false;
        }
        
        public HandCardSlot GetCardSlot(Card card)
        {
            foreach (var cardSlot in _cardSlots)
            {
                if (cardSlot.IsEmpty)
                {
                    continue;
                }

                if (cardSlot.CurrentCardUi.Card == card)
                {
                    return cardSlot;
                }
            }

            throw new ArgumentException();
        }

        public void PlayHighlightOnAllCards()
        {
            foreach (var cardSlot in _cardSlots)
            {
                if (cardSlot.IsEmpty)
                {
                    continue;
                }
                
                cardSlot.PlayHighlight();
            }
        }
        
        public void StopHighlightOnAllCards()
        {
            foreach (var cardSlot in _cardSlots)
            {
                if (cardSlot.IsEmpty)
                {
                    continue;
                }
                
                cardSlot.StopHighlight();
            }
        }

        public void SetEnableCardSelection(bool enable)
        {
            if (enable)
            {
                EnableCardSelection();
            }
            else
            {
                DisableCardSelection();
            }
        }
        
        private void EnableCardSelection()
        {
            foreach (var cardSlot in _cardSlots)
            {
                if (cardSlot.IsEmpty)
                {
                    continue;
                }
                
                cardSlot.EnableCardSelection();
            }
        }
        
        private void DisableCardSelection()
        {
            foreach (var cardSlot in _cardSlots)
            {
                if (cardSlot.IsEmpty)
                {
                    continue;
                }
                
                cardSlot.DisableCardSelection();
            }
        }
    }
}