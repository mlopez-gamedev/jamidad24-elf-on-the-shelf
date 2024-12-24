using Unity.VisualScripting;
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

            GameUi.Instance.OnSelectCard += OnSelectCard;
        }

        private void OnSelectCard(CardUi cardUi)
        {
            if (cardUi != _currentCardUi)
            {
                return;
            }

            GameUi.Instance.OnSelectCard -= OnSelectCard;
            _currentCardUi = null;
        }

        public void PlayHighlight()
        {
            _currentCardUi?.PlayHighlight();
        }
        
        public void StopHighlight()
        {
            _currentCardUi?.StopHighlight();
        }

        public void EnableCardSelection()
        {
            _currentCardUi.EnableSelection();
        }
        
        public void DisableCardSelection()
        {
            _currentCardUi.DisableSelection();
        }
    }
}