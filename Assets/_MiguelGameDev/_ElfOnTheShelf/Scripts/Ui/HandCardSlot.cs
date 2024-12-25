using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class HandCardSlot : MonoBehaviour
    {
        [ShowInInspector, HideInEditorMode] private CardUi? _currentCardUi;
        
        public bool IsEmpty => _currentCardUi == null;
        
        public CardUi CurrentCardUi => _currentCardUi;
        
        public void AddCard(CardUi cardUi)
        {
            Assert.IsTrue(IsEmpty);
            _currentCardUi = cardUi;
            cardUi.transform.SetParent(transform);

            GameUi.Instance.OnSelectCard += OnSelectCard;
        }

        public void RemoveCard()
        {
            _currentCardUi = null;
        }

        private void OnSelectCard(CardUi cardUi)
        {
            if (cardUi != _currentCardUi)
            {
                return;
            }

            GameUi.Instance.OnSelectCard -= OnSelectCard;
            RemoveCard();
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
            _currentCardUi?.EnableSelection(SelectCard, CancelSelection);

            void SelectCard(CardUi cardUi)
            {
                GameUi.Instance.SelectActionCard((ActionCardUi)cardUi);
            }
            
            void CancelSelection(CardUi cardUi)
            {
                GameUi.Instance.CancelCardSelection((ActionCardUi)cardUi);
            }
        }
        
        public void DisableCardSelection()
        {
            _currentCardUi?.DisableSelection();
        }
    }
}