using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class RunPanel : MonoBehaviour, IDropHandler
    {
        private readonly Vector3 CardScale = new Vector3(0.8f, 0.8f, 0.8f);
        
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

            if (!eventData.pointerDrag.TryGetComponent<CardUi>(out var cardUi))
            {
                return;
            }

            var slot = new GameObject();
            slot.AddComponent<RectTransform>();
            slot.AddComponent<LayoutElement>();
            slot.transform.SetParent(_cardContainer);
            slot.name = $"CardSlot_{_cardContainer.childCount}";
            cardUi.Drop(slot.transform, CardScale, PlayCard);

            void PlayCard()
            {
                GameUi.Instance.PlayCard(cardUi);
            }
        }
    }
}