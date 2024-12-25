using UnityEngine;
using UnityEngine.EventSystems;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class DiscardPilePanel : MonoBehaviour, IDropHandler
    {
        public readonly Vector3 CardScale = Vector3.one; //new Vector3(0.8f, 0.8f, 0.8f);
        
        [SerializeField] private RectTransform _cardContainer;
        [SerializeField] private Highlight _highlight;

        private bool _canDrop;
        
        public void SetEnableDrop(bool enable)
        {
            if (enable)
            {
                EnableDrop();
            }
            else
            {
                DisableDrop();
            }
        }
        
        private void EnableDrop()
        {
            _canDrop = true;
            _highlight.Play();
        }
        
        private void DisableDrop()
        {
            _canDrop = false;
            _highlight.Stop();
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            if (!_canDrop)
            {
                return;
            }
            
            if (eventData.pointerDrag == null)
            {
                return;
            }

            if (!eventData.pointerDrag.TryGetComponent<ActionCardUi>(out var cardUi))
            {
                return;
            }
            
            cardUi.Drop(_cardContainer, CardScale, DiscardCard);

            void DiscardCard()
            {
                GameUi.Instance.PlayerDiscardCard(cardUi);
            }
        }
    }
}