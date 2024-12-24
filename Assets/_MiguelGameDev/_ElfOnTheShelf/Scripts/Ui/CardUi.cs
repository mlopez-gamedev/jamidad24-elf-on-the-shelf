using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

using UnityEngine.EventSystems;

namespace MiguelGameDev.ElfOnTheShelf
{
    public abstract class CardUi : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Image _interactableBackground;
        [SerializeField] private Image _picture;
        [SerializeField] private Image _reversePicture;
        [SerializeField] private Highlight _highlight;
        
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private bool _flippedUp = false;
        
        private bool _canSelect;
        private bool _isSelected;
        public Card Card { get; private set; }
        public bool IsFlippedUp => _flippedUp;

        private void Awake()
        {
            _interactableBackground.raycastTarget = false;
            _rectTransform = GetComponent<RectTransform>();
            _canvas = _rectTransform.root.GetComponent<Canvas>();
        }
        
        protected void Setup(Card card)
        {
            Card = card;
            _picture.sprite = card.Picture;
        }

        [Button]
        public Tween FlipUp()
        {
            if (_flippedUp)
            {
                return default;
            }
            
            _flippedUp = true;

            return DOTween.Sequence()
                .Append(transform.DOScaleX(0f, 0.1f).SetEase(Ease.InQuad))
                .AppendCallback(HideReverse)
                .Append(transform.DOScaleX(1f, 0.1f).SetEase(Ease.OutQuad));

            void HideReverse()
            {
                _reversePicture.gameObject.SetActive(false);
            }
        }
        
        [Button]
        public Tween FlipDown()
        {
            if (!_flippedUp)
            {
                return default;
            }

            _flippedUp = false;
            
            return DOTween.Sequence()
                .Append(transform.DOScaleX(0f, 0.1f).SetEase(Ease.InQuad))
                .AppendCallback(ShowReverse)
                .Append(transform.DOScaleX(1f, 0.1f).SetEase(Ease.OutQuad));

            void ShowReverse()
            {
                _reversePicture.gameObject.SetActive(true);
            }
        }
        
        public void EnableSelection()
        {
            _canSelect = true;
            PlayHighlight();
        }
        
        public void DisableSelection()
        {
            _canSelect = false;
            StopHighlight();
        }

        public void PlayHighlight()
        {
            _interactableBackground.raycastTarget = true;
            _highlight.Play();
        }
        
        public void StopHighlight()
        {
            _interactableBackground.raycastTarget = false;
            _highlight.Stop();
        }

        public void Drop(Transform parent, Vector3 scale, TweenCallback callback)
        {
            if (!_isSelected)
            {
                return;
            }
            _isSelected = false;
            
            transform.SetParent(parent, true);
            transform.DOLocalMove(Vector3.zero, 0.1f).OnComplete(callback);
            if (transform.localScale != scale)
            {
                transform.DOScale(scale, 0.1f);
            }
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_canSelect)
            {
                return;
            }
            
            GameUi.Instance.SelectCard(this);
            _isSelected = true;
            StopHighlight();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isSelected)
            {
                return;
            }
            _isSelected = false;
            GameUi.Instance.CancelCardSelection(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isSelected)
            {
                return;
            }
            _rectTransform.anchoredPosition += eventData.delta * _canvas.scaleFactor;
        }
    }
}