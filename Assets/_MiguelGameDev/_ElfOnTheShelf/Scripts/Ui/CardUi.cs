using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using LeTai.TrueShadow;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

namespace MiguelGameDev.ElfOnTheShelf
{
    public abstract class CardUi : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        [SerializeField] private Image _interactableBackground;
        [SerializeField] protected Image _picture;
        [SerializeField] private Image _reversePicture;
        [SerializeField] private Highlight _highlight;
        [SerializeField] private TrueShadow _shadow;
        
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private bool _flippedUp = false;
        
        [ShowInInspector, HideInEditorMode] protected bool _canSelect;
        [ShowInInspector, HideInEditorMode] protected bool _isSelected;
        
        [ShowInInspector, HideInEditorMode] private bool _isClickEnabled;
        private Action<CardUi> _onClickAction;
        
        public RectTransform RectTransform => _rectTransform ??= (RectTransform)transform;
        [ShowInInspector, HideInEditorMode] public Card Card { get; private set; }
        [ShowInInspector, HideInEditorMode] public bool IsFlippedUp => _flippedUp;

        private Action<CardUi> _beginDragAction;
        private Action<CardUi> _endDragAction;
        
        private void Awake()
        {
            _interactableBackground.raycastTarget = false;
            _canvas = RectTransform.root.GetComponent<Canvas>();
        }
        
        protected void Setup(Card card)
        {
            Card = card;
            _picture.sprite = card.Picture;

            ForceRepaint();
        }

        private async void ForceRepaint()
        {
            gameObject.SetActive(false);
            await UniTask.Yield();
            gameObject.SetActive(true);
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
        
        public void EnableSelection(Action<CardUi> beginDragAction, Action<CardUi> endDragAction)
        {
            _canSelect = true;
            
            _beginDragAction = beginDragAction;
            _endDragAction = endDragAction;
            
            EnableInteraction();
        }
        
        public void DisableSelection()
        {
            _canSelect = false;
            DisableInteraction();
        }

        public void EnableInteraction()
        {
            _interactableBackground.raycastTarget = true;
            _highlight.Play();
        }
        
        public void DisableInteraction()
        {
            _interactableBackground.raycastTarget = false;
            _highlight.Stop();
        }
        
        public void PlayHighlight()
        {
            _highlight.Play();
        }
        
        public void StopHighlight()
        {
            _highlight.Stop();
        }

        public void StopSelection()
        {
            _isSelected = false;
        }
        
        public async void Drop(Transform parent, Vector3 scale, TweenCallback callback)
        {
            if (!_isSelected)
            {
                return;
            }
            _isSelected = false;
            
            await UniTask.Yield();
            
            transform.SetParent(parent, true);
            //float duration = Vector3.Distance(transform.position, parent.position) / 1200f;
            RectTransform.DOAnchorPos(Vector2.zero, 0.2f).OnComplete(callback);
            if (transform.localScale != scale)
            {
                transform.DOScale(scale, 0.2f);
            }
            HideShadow();
        }
        
        public void OnBeginDrag(PointerEventData _)
        {
            if (!_canSelect)
            {
                return;
            }
            
            _beginDragAction?.Invoke(this);
            _isSelected = true;
            DisableInteraction();
        }

        public void OnEndDrag(PointerEventData _)
        {
            if (!_isSelected)
            {
                return;
            }
            _isSelected = false;
            _endDragAction?.Invoke(this);
            
            _beginDragAction = null;
            _endDragAction = null;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isSelected)
            {
                return;
            }
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void CancelDrag()
        {
            _isSelected = false;
            _endDragAction?.Invoke(this);
            
            _beginDragAction = null;
            _endDragAction = null;
            
            _canSelect = false;
        }
        
        public void EnableClick(Action<CardUi> onClickAction)
        {
            _isClickEnabled = true;
            _onClickAction = onClickAction;
            EnableInteraction();
        }

        public void DisableClick()
        {
            _isClickEnabled = false;
            _onClickAction = null;
            DisableInteraction();
        }

        public void OnPointerClick(PointerEventData _)
        {
            _onClickAction?.Invoke(this);
        }

        public void HideShadow()
        {
            _shadow.enabled = false;
        }
    }
}