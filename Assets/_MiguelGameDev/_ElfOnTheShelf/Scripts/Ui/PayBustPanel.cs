using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class PayBustPanel : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private RectTransform _panel;
        
        [SerializeField] private RectTransform _bustCardSlot;
        
        [SerializeField] private HighlightPayOptionSlot _payHandPaySlot;
        [SerializeField] private HighlightPayOptionSlot _payTrickCardSlot;
        [SerializeField] private HighlightPayOptionSlot _payGoalSlot;
        [SerializeField] private HighlightPayOptionSlot _payDeckCardsSlot;
        
        [SerializeField] private RectTransform _handCardsSlot;
        [SerializeField] private HandCards _handCards;
        
        [SerializeField] private RectTransform _goalsCardsSlot;
        [SerializeField] private GoalCardsPanel _goalsCards;
        
        [SerializeField] private Button _backButton;

        [ShowInInspector, HideInEditorMode] private BustCardUi _bustCardUi;
        [ShowInInspector, HideInEditorMode] private ActionCardUi[] _trickCardsUi;
        [ShowInInspector, HideInEditorMode] private GoalCardUi[] _goalsCardsUi;
        
        private UniTaskCompletionSource<PayBustOption> _completionSource;

        private void Start()
        {
            _panel.gameObject.SetActive(false);
            _background.gameObject.SetActive(false);
            _backButton.gameObject.SetActive(false);
            
            _background.SetAlpha(0);
            
            _payHandPaySlot.Setup(PayHand);
            _payTrickCardSlot.Setup(PayTrickCard);
            _payGoalSlot.Setup(PayGoal);
            _payDeckCardsSlot.Setup(PayDeckCards);
            
            _payHandPaySlot.gameObject.SetActive(false);
            _payTrickCardSlot.gameObject.SetActive(false);
            _payGoalSlot.gameObject.SetActive(false);
            _payDeckCardsSlot.gameObject.SetActive(false);
            _backButton.gameObject.SetActive(false);
            
            _backButton.onClick.AddListener(OnBackButtonClick);
        }

        public UniTask<PayBustOption> Show(BustCardUi bustCardUi, ActionCard[] trickCards)
        {
            _completionSource = new UniTaskCompletionSource<PayBustOption>();
            
            _panel.gameObject.SetActive(true);
            _background.gameObject.SetActive(true);
            _bustCardSlot.gameObject.SetActive(true);

            ShowButtons();
            
            _bustCardUi = bustCardUi;
            _trickCardsUi = new ActionCardUi[trickCards.Length];
            for (int i = 0; i < trickCards.Length; ++i)
            {
                _trickCardsUi[i] = (ActionCardUi)CardUiFactory.Instance.GetCardUi(trickCards[i]);
                _trickCardsUi[i].EnableClick(ConfirmPayTrickCard);
            }

            var cardGoals = Game.Instance.Player.CompletedGoalCards;
            _goalsCardsUi = new GoalCardUi[cardGoals.Count];
            for (int i = 0; i < cardGoals.Count; ++i)
            {
                _goalsCardsUi[i] = (GoalCardUi)CardUiFactory.Instance.GetCardUi(cardGoals[i]);
                _goalsCardsUi[i].EnableClick(ConfirmPayGoal);
            }
            
            _payTrickCardSlot.SetEnable(_trickCardsUi.Length > 0);
            _payGoalSlot.SetEnable(Game.Instance.Player.CompletedGoalCards.Count > 0);
            _payDeckCardsSlot.SetEnable(Game.Instance.Player.DeckCardsLeft() >= 5);
            
            _bustCardUi.transform.SetParent(_bustCardSlot, true);
            
            DOTween.Sequence()
                .Append(_background.DOFade(0.8f, 0.5f))
                .Join(DOCenterAtSlot(_bustCardUi.RectTransform, 0.5f))
                .OnComplete(ActivateBustCard);
            
            return _completionSource.Task;
        }
        
        void ActivateBustCard()
        {
            _bustCardUi.EnableSelection(OnBeginDrag, OnEndDrag);
                
            void OnBeginDrag(CardUi cardUi)
            {
                _payHandPaySlot.PlayHighlight();
                _payTrickCardSlot.PlayHighlight();
                _payGoalSlot.PlayHighlight();
                _payDeckCardsSlot.PlayHighlight();

                cardUi.DisableSelection();
            }
            
            void OnEndDrag(CardUi cardUi)
            {
                _payHandPaySlot.StopHighlight();
                _payTrickCardSlot.StopHighlight();
                _payGoalSlot.StopHighlight();
                _payDeckCardsSlot.StopHighlight();

                DOCenterAtSlot(cardUi.RectTransform, 0.1f)
                    .OnComplete(ActivateBustCard);
            }
        }
        
        private void ShowButtons()
        {
            _payHandPaySlot.gameObject.SetActive(true);
            _payTrickCardSlot.gameObject.SetActive(true);
            _payGoalSlot.gameObject.SetActive(true);
            _payDeckCardsSlot.gameObject.SetActive(true);
            
            _backButton.gameObject.SetActive(false);
        }
        
        private void HideButtons()
        {
            _payHandPaySlot.StopHighlight();
            _payTrickCardSlot.StopHighlight();
            _payGoalSlot.StopHighlight();
            _payDeckCardsSlot.StopHighlight();
            
            _payHandPaySlot.gameObject.SetActive(false);
            _payTrickCardSlot.gameObject.SetActive(false);
            _payGoalSlot.gameObject.SetActive(false);
            _payDeckCardsSlot.gameObject.SetActive(false);
            
            _backButton.gameObject.SetActive(true);
        }
        
        private Tween DOCenterAtSlot(RectTransform rectTransform, float duration)
        {
            return DOTween.Sequence()
                .Join(rectTransform.DOAnchorPos(Vector2.zero, duration))
                .Join(rectTransform.DORotate(Vector2.zero, duration));
        }
        
        private async void PayHand()
        {
            UniTask[] payTasks = new[]
            {
                GameUi.Instance.MoveCardToDiscardPanel(_bustCardUi),
                _background.DOFade(0, 0.2f).AsyncWaitForCompletion().AsUniTask()
            };
            
            await UniTask.WhenAll(payTasks);
            DisableAll();
            _completionSource.TrySetResult(new PayBustOption(
                EPayBustOption.DiscardHand));
        }
        
        private void PayTrickCard()
        {
            HideButtons();
            _bustCardSlot.gameObject.SetActive(false);
            
            foreach (var trickCardUi in _trickCardsUi)
            {
                trickCardUi.EnableClick(ConfirmPayTrickCard);
            }
            
            _handCards.transform.SetParent(transform, true);
            DOCenterAtSlot((RectTransform)_handCards.transform, 0.2f);
        }

        private async void ConfirmPayTrickCard(CardUi cardUi)
        {
            foreach (var trickCardUi in _trickCardsUi)
            {
                trickCardUi.DisableClick();
            }
            
            _handCards.transform.SetParent(_handCardsSlot, true);
            DOCenterAtSlot((RectTransform)_handCards.transform, 0.2f);
            
            UniTask[] payTasks = new[]
            {
                GameUi.Instance.MoveCardToDiscardPanel(_bustCardUi),
                _background.DOFade(0, 0.2f).AsyncWaitForCompletion().AsUniTask()
            };

            await UniTask.WhenAll(payTasks);
            DisableAll();
            _completionSource.TrySetResult(new PayBustOptionWithCard(
                EPayBustOption.DiscardTrickCard, cardUi));
        }
        
        private void PayGoal()
        {
            HideButtons();
            _bustCardSlot.gameObject.SetActive(false);
            
            foreach (var goalCardUi in _goalsCardsUi)
            {
                goalCardUi.EnableClick(ConfirmPayGoal);
            }
            
            _goalsCards.transform.SetParent(transform, true);
            DOTween.Sequence()
                .Join(DOCenterAtSlot((RectTransform)_goalsCards.transform, 0.2f))
                .Join(_goalsCards.transform.DOScale(1f, 0.2f));
        }

        private async void ConfirmPayGoal(CardUi cardUi)
        {
            foreach (var goalCardUi in _goalsCardsUi)
            {
                goalCardUi.DisableClick();
            }
            
            _goalsCards.transform.SetParent(_goalsCardsSlot, true);
            DOTween.Sequence()
                .Join(DOCenterAtSlot((RectTransform)_goalsCards.transform, 0.2f))
                .Join(_goalsCards.transform.DOScale(0.35f, 0.2f));
            
            UniTask[] payTasks = new[]
            {
                GameUi.Instance.MoveCardToDiscardPanel(_bustCardUi),
                _background.DOFade(0, 0.2f).AsyncWaitForCompletion().AsUniTask()
            };
            
            await UniTask.WhenAll(payTasks);
            DisableAll();
            _completionSource.TrySetResult(new PayBustOptionWithCard(
                EPayBustOption.RemoveGoal, cardUi));
        }
        
        private async void PayDeckCards()
        {
            UniTask[] payTasks = new[]
            {
                GameUi.Instance.MoveCardToDiscardPanel(_bustCardUi),
                _background.DOFade(0, 0.2f).AsyncWaitForCompletion().AsUniTask()
            };
            
            await UniTask.WhenAll(payTasks);
            DisableAll();
            _completionSource.TrySetResult(new PayBustOption(
                EPayBustOption.DiscardDeckCards));
        }
        
        private void OnBackButtonClick()
        {
            _payHandPaySlot.StopHighlight();
            _payTrickCardSlot.StopHighlight();
            _payGoalSlot.StopHighlight();
            _payDeckCardsSlot.StopHighlight();
            
            ShowButtons();
            _bustCardSlot.gameObject.SetActive(true);
            _bustCardUi.transform.SetParent(_bustCardSlot, false);
            _bustCardUi.RectTransform.anchoredPosition = Vector2.zero;

            foreach (var trickCardUi in _trickCardsUi)
            {
                trickCardUi.DisableClick();
            }
            
            foreach (var goalCardUi in _goalsCardsUi)
            {
                goalCardUi.DisableClick();
            }
            
            if (_handCards.transform.parent != _handCardsSlot)
            {
                _handCards.transform.SetParent(_handCardsSlot, true);
                DOCenterAtSlot((RectTransform)_handCards.transform, 0.2f);
            }

            if (_goalsCards.transform.parent != _goalsCardsSlot)
            {
                _goalsCards.transform.SetParent(_goalsCardsSlot, true);
                DOTween.Sequence()
                    .Join(DOCenterAtSlot((RectTransform)_goalsCards.transform, 0.2f))
                    .Join(_goalsCards.transform.DOScale(0.35f, 0.2f));
            }

            ActivateBustCard();
        }

        private void DisableAll()
        {
            foreach (var trickCardUi in _trickCardsUi)
            {
                trickCardUi.DisableClick();
            }
            
            foreach (var trickCardUi in _goalsCardsUi)
            {
                trickCardUi.DisableClick();
            }
            
            _bustCardUi.DisableSelection();
            
            _panel.gameObject.SetActive(false);
            _background.gameObject.SetActive(false);
        }
    }
}