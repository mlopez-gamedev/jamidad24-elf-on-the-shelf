using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class PayGoalPanel : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private RectTransform _panel;
        
        [SerializeField] private RectTransform _goalCardSlot;
        
        [SerializeField] private PayOptionSlot _magicalPortalPaySlot;
        [SerializeField] private PayOptionSlot _trickCardPaySlot;
        
        [SerializeField] private HandCards _handCards;
        [SerializeField] private RectTransform _magicalPortalSlot;
        [SerializeField] private MagicalPortalCards _magicalPortal;
        
        [ShowInInspector, HideInEditorMode] private HandCardSlot _trickCardHandSlot;
        [ShowInInspector, HideInEditorMode] private ActionCardUi _trickCardUi;
        [ShowInInspector, HideInEditorMode] private GoalCardUi _goalCardUi;

        private UniTaskCompletionSource<bool> _completionSource;

        private void Start()
        {
            _panel.gameObject.SetActive(false);
            _background.gameObject.SetActive(false);
            
            _background.SetAlpha(0);
            
            _magicalPortalPaySlot.Setup(SendToMagicalPortal);
            _trickCardPaySlot.Setup(Pay);
        }

        public UniTask<bool> Show(GoalCardUi goalCardUi, ActionCard trickCard)
        {
            _goalCardUi = goalCardUi;
            _trickCardHandSlot = _handCards.GetCardSlot(trickCard);
            _trickCardUi = (ActionCardUi)_trickCardHandSlot.CurrentCardUi;

            _completionSource = new UniTaskCompletionSource<bool>();
            
            _panel.gameObject.SetActive(true);
            _background.gameObject.SetActive(true);
            
            _goalCardUi.transform.SetParent(_goalCardSlot, true);
            _trickCardUi.transform.SetParent(_trickCardPaySlot.transform, true);
            _magicalPortal.transform.SetParent(_magicalPortalPaySlot.transform, true);
            
            DOTween.Sequence()
                .Append(_background.DOFade(0.8f, 0.5f))
                .Join(DOCenterAtSlot(_goalCardUi.RectTransform, 0.5f))
                .Join(DOCenterAtSlot(_trickCardUi.RectTransform, 0.8f))
                .Join(DOCenterAtSlot(_magicalPortal.RectTransform, 1.0f))
                .OnComplete(Activate);
            
            return _completionSource.Task;

            void Activate()
            {
                _goalCardUi.EnableSelection(OnBeginDrag, OnEndDrag);
            }
            
            void OnBeginDrag(CardUi cardUi)
            {
                _magicalPortal.PlayHighlight();
                _trickCardUi.PlayHighlight();
                cardUi.DisableSelection();
            }
            
            void OnEndDrag(CardUi cardUi)
            {
                Debug.Log("PayGoalPanel OnEndDrag");
                _magicalPortal.StopHighlight();
                _trickCardUi.StopHighlight();
                DOCenterAtSlot(cardUi.RectTransform, 0.1f)
                    .OnComplete(Activate);
            }
        }

        private Tween DOCenterAtSlot(RectTransform rectTransform, float duration)
        {
            return DOTween.Sequence()
                .Join(rectTransform.DOAnchorPos(Vector2.zero, duration))
                .Join(rectTransform.DORotate(Vector2.zero, duration));
        }

        public async void Pay()
        {
            _goalCardUi.StopSelection();
            _magicalPortal.transform.SetParent(_magicalPortalSlot, true);
            _trickCardHandSlot.RemoveCard();
            
            UniTask[] payTasks = new[]
            {
                GameUi.Instance.MoveCardToGoalPanel(_goalCardUi),
                GameUi.Instance.MoveCardToDiscardPanel(_trickCardUi),
                DOTween.Sequence()
                    .Append(_background.DOFade(0, 0.2f))
                    .Join(DOCenterAtSlot(_magicalPortal.RectTransform, 0.5f))
                    .AsyncWaitForCompletion().AsUniTask()
            };

            await UniTask.WhenAll(payTasks);
            _panel.gameObject.SetActive(false);
            _background.gameObject.SetActive(false);

            _completionSource.TrySetResult(true);
        }

        public async void SendToMagicalPortal()
        {
            _goalCardUi.StopSelection();
            _background.DOFade(1, 0.2f);
            await GameUi.Instance.MoveCardToMagicalPortal(_goalCardUi);
            
            _magicalPortal.transform.SetParent(_magicalPortalSlot.transform, true);
            _trickCardUi.transform.SetParent(_trickCardHandSlot.transform, true);
            DOTween.Sequence()
                .Join(DOCenterAtSlot(_trickCardUi.RectTransform, 0.5f))
                .Join(DOCenterAtSlot(_magicalPortal.RectTransform, 0.5f));
            
            _panel.gameObject.SetActive(false);
            _background.gameObject.SetActive(false);
            
            _completionSource.TrySetResult(false);
        }
    }
}