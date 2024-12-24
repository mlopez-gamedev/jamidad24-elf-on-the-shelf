using UnityEngine;
using UnityEngine.Assertions;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class HandCardSlot : MonoBehaviour
    {
        private CardUi? _currentCardUi;
        
        public bool IsEmpty => _currentCardUi == null;
        
        public void AddCard(CardUi cardUi)
        {
            Assert.IsTrue(IsEmpty);
            _currentCardUi = cardUi;
            cardUi.transform.SetParent(transform);
        }
    }
}