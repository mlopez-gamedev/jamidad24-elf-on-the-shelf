using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

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
    }
}