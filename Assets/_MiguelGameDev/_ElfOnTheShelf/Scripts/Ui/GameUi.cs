using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class GameUi : SingletonBehaviour<GameUi>
    {
        [SerializeField] private CardUiFactory _cardUiFactory;
        [SerializeField] private DeckPanel _deckPanel;
        [SerializeField] private Transform _freeCards;
        [SerializeField] private HandCards _handCards;
        [SerializeField] private RunPanel _runPanel;
        [SerializeField] private GoalCardsPanel _goalCardsPanel;
        [SerializeField] private DiscardPilePanel _discardPilePanel;
        [SerializeField] private MagicalPortalCards _magicalPortalCards;
        [SerializeField] private DeckAmountUi _deckAmountUi;
        
        [SerializeField] private PayGoalPanel _payGoalPanel;
        [SerializeField] private PayBustPanel _payBustPanel;
        
        private ActionCardUi _selectedActionCardUi;
        [ShowInInspector, HideInEditorMode] public ActionCardUi SelectedActionCardUi => _selectedActionCardUi;
        [ShowInInspector, HideInEditorMode] public GoalCardUi DrawnGoalCardUi { get; set; }
        [ShowInInspector, HideInEditorMode] public BustCardUi DrawnBustCardUi { get; set; }
        
        public event System.Action<ActionCardUi> OnSelectCard;
        public event System.Action<ActionCardUi> OnCancelCardSelection;
        public event System.Action<ActionCardUi> OnPlayerPlayCard;
        public event System.Action<ActionCardUi> OnPlayerDiscardCard;
        
        public void Init(int amount)
        {
            _deckAmountUi.SetAmount(amount);
        }
        
        public void PlayDeckAmount(int amount)
        {
            _deckAmountUi.PlaySetAmount(amount);
            _deckPanel.SetHidden(amount == 0);
        }
        public void EnableAndHighlightCardSelection(bool onlyHighlightHideCards)
        {
            SetEnableCardSelection(true);
            // Highlight
        }
        
        public void SetEnableCardSelection(bool enable)
        {
            _handCards.SetEnableCardSelection(enable);
        }
        
        public void SetEnableDropOnRunPanel(bool enable)
        {
            _runPanel.SetEnableDrop(enable);
        }
        
        public void SetEnableDropOnDiscardPilePanel(bool enable)
        {
            _discardPilePanel.SetEnableDrop(enable);
        }
        
        public void SetEnableDeck(bool enable)
        {
            // TODO
            // always highlight on off
        }
        
        public void SetHighlightRun(bool highlight)
        {
            // TODO
            // Highlight
        }
        
        public void SetEnableGoals(bool enable)
        {
            // TODO
            // always highlight
        }
        
        public void DisableAll()
        {
            SetEnableCardSelection(false);
            SetEnableDeck(false);
            SetEnableGoals(false);
            SetEnableDropOnRunPanel(false);
            SetEnableDropOnDiscardPilePanel(false);
        }

        public void ShowCardOrderPanel()
        {
            // TODO
        }

        public UniTask<bool> ShowPayGoalPanel(ActionCard trickCard)
        {
            return _payGoalPanel.Show(DrawnGoalCardUi, trickCard);
        }
        
        public UniTask<PayBustOption> ShowPayBustPanel(ActionCard[] trickCards)
        {
            return _payBustPanel.Show(DrawnBustCardUi, trickCards);
        }
        
        public UniTask<HandCardSlot> DrawCardToHand(Card card)
        {
            var cardUi = _deckPanel.DrawCard(card);
            return MoveCardToHand(cardUi);
        }

        public UniTask DiscardTopDeckCards(Card[] cards)
        {
            var tasks = new UniTask[cards.Length];
            for (int i = 0; i < cards.Length; i++)
            {
                tasks[i] = DiscardCard(cards[i], i * 500);
            }
            
            return UniTask.WhenAll(tasks);
            

            async UniTask DiscardCard(Card card, int delay)
            {
                if (delay > 0)
                {
                    await Task.Delay(delay);
                }

                var cardUi = await PeekTopCard(card);
                if (card.Type == ECardType.Action)
                {
                    await MoveCardToDiscardPanel(cardUi);
                    return;
                }

                await MoveCardToMagicalPortal(cardUi);
            }
        }
        
        public async UniTask<CardUi> PeekTopCard(Card card)
        {
            var cardUi = _deckPanel.DrawCard(card);
            cardUi.transform.SetParent(_freeCards, true);
            
            var moveToHandSequence = DOTween.Sequence()
                .Join(cardUi.transform.DOMoveY(120f, 0.2f));

            if (!cardUi.IsFlippedUp)
            {
                moveToHandSequence.Join(cardUi.FlipUp());
            }
            
            await moveToHandSequence.AsyncWaitForCompletion();
            return cardUi;
        }
        
        public async UniTask CompleteGoal(GoalCard card)
        {
            var cardUi = await _deckPanel.SearchCard(card) as GoalCardUi;
            await MoveCardToGoalPanel(cardUi);
        }

        public void SelectActionCard(ActionCardUi cardUi)
        {
            _selectedActionCardUi = cardUi;
            
            cardUi.transform.SetParent(_freeCards, true);
            cardUi.transform.DORotate(Vector3.zero, 0.05f).SetEase(Ease.Flash);
            
            OnSelectCard?.Invoke(cardUi);
        }

        public void PlayCard(ActionCardUi cardUi)
        {
            if (cardUi != _selectedActionCardUi)
            {
                return;
            }
            
            Audio2dService.Instance.PlayAudio(EAudioEvent.PlayActionSfx);
            DisableAll();
            
            _selectedActionCardUi = null;
            OnPlayerPlayCard?.Invoke(cardUi);
        }
        
        public void PlayerDiscardCard(ActionCardUi cardUi)
        {
            if (cardUi != _selectedActionCardUi)
            {
                return;
            }
            
            Audio2dService.Instance.PlayAudio(EAudioEvent.DiscardSfx);
            DisableAll();
            
            _selectedActionCardUi = null;
            OnPlayerDiscardCard?.Invoke(cardUi);
        }
        
        public void CancelCardSelection(ActionCardUi cardUi)
        {
            if (cardUi != _selectedActionCardUi)
            {
                return;
            }

            DisableAll();
            
            _selectedActionCardUi = null;
            OnCancelCardSelection?.Invoke(cardUi);
        }

        public UniTask ShuffleDeck()
        {
            return _deckPanel.PlayShuffle();
        }

        public UniTask<HandCardSlot> MoveCardToHand(ActionCard card)
        {
            var cardUi = _cardUiFactory.GetCardUi(card);
            return MoveCardToHand((ActionCardUi)cardUi);
        }
        
        public async UniTask<HandCardSlot> MoveCardToHand(CardUi cardUi)
        {
            if (!_handCards.TryGetFirstEmptySlot(out var cardSlot))
            {
                return null;
            }
            
            cardUi.transform.SetParent(_freeCards, true);
            
            var duration = Mathf.Clamp(
                Vector3.Distance(cardUi.transform.position, cardSlot.transform.position) / 800f,
                0.1f,
                0.6f);
            
            var moveToHandSequence = DOTween.Sequence()
                .Join(cardUi.transform.DOMove(cardSlot.transform.position, duration))
                .Join(cardUi.transform.DORotate(cardSlot.transform.rotation.eulerAngles, duration));

            if (!cardUi.IsFlippedUp)
            {
                moveToHandSequence.Join(cardUi.FlipUp());
            }
            
            await moveToHandSequence.AsyncWaitForCompletion();
            cardSlot.AddCard(cardUi);
            return cardSlot;
        }

        public async UniTask MoveCardToGoalPanel(GoalCardUi cardUi)
        {
            var cardSlot = _goalCardsPanel.GetEmptyGoalCardSlot(((GoalCard)cardUi.Card).Suit.Id);
            cardUi.transform.SetParent(_freeCards, true);
            
            var duration = Mathf.Clamp(
                Vector3.Distance(cardUi.transform.position, cardSlot.transform.position) / 800f,
                0.1f,
                0.6f);
            
            var moveToHandSequence = DOTween.Sequence()
                .Join(cardUi.transform.DOMove(cardSlot.transform.position, duration))
                .Join(cardUi.transform.DOScale(_goalCardsPanel.transform.localScale, duration));
            
            if (!cardUi.IsFlippedUp)
            {
                moveToHandSequence.Join(cardUi.FlipUp());
            }
            
            await moveToHandSequence.AsyncWaitForCompletion();
            Audio2dService.Instance.PlayAudio(EAudioEvent.GoalSfx);
            cardSlot.AddCard(cardUi);
        }

        public UniTask MoveCardToMagicalPortal(Card card)
        {
            var cardUi = _cardUiFactory.GetCardUi(card);
            return MoveCardToMagicalPortal(cardUi);
        }
        
        public async UniTask MoveCardToMagicalPortal(CardUi cardUi)
        {
            cardUi.transform.SetParent(_freeCards, true);
            await DOTween.Sequence()
                .Join(cardUi.transform.DOMove(_magicalPortalCards.transform.position, 0.7f))
                .Join(cardUi.transform.DORotate(Vector3.zero, 0.5f))
                .Join(cardUi.transform.DOScale(0.25f, 0.7f).SetEase(Ease.InQuad))
                .AsyncWaitForCompletion();
            Audio2dService.Instance.PlayAudio(EAudioEvent.MagicalPortalSfx);
            _magicalPortalCards.AddCard(cardUi);
        }

        public async UniTask MoveCardsFromMagicalPortalToDeck()
        {
            var moveTasks = new List<UniTask>();

            int count = 0;
            while(_magicalPortalCards.TryPopCard(_freeCards, out var cardUi))
            {
                moveTasks.Add(MoveCardFromMagicalPortalToDeck(cardUi, count * 200));
                ++count;
            };
            
            await UniTask.WhenAll(moveTasks);

            async UniTask MoveCardFromMagicalPortalToDeck(CardUi cardUi, int delay)
            {
                if (delay > 0)
                {
                    await UniTask.Delay(delay);
                }
                await MoveCardToDeck(cardUi);
                CardUiFactory.Instance.RemoveCardUi(cardUi.Card);
            }
        }
        
        public async UniTask MoveCardToDeck(CardUi cardUi)
        {
            if (cardUi.transform.parent != _freeCards)
            {
                cardUi.transform.SetParent(_freeCards, true);
            }

            var duration = Mathf.Clamp(
                Vector3.Distance(cardUi.transform.position, _deckPanel.transform.position) / 300f,
                0.1f,
                0.6f);
            
            var moveToHandSequence = DOTween.Sequence()
                .Join(cardUi.transform.DOMove(_deckPanel.transform.position, duration))
                .Join(cardUi.transform.DOScale(_deckPanel.transform.localScale, duration));
            
            if (cardUi.IsFlippedUp)
            {
                moveToHandSequence.Join(cardUi.FlipDown());
            }
            
            await moveToHandSequence.AsyncWaitForCompletion();
            Destroy(cardUi.gameObject);
        }
        
        public async UniTask MoveCardToDiscardPanel(CardUi cardUi, int delay = 0)
        {
            Audio2dService.Instance.PlayAudio(EAudioEvent.DiscardSfx);
            cardUi.RectTransform.SetParent(_discardPilePanel.transform, true);
            float duration = Vector3.Distance(cardUi.transform.position, _discardPilePanel.transform.position) / 800f;

            if (delay > 0)
            {
                await UniTask.Delay(delay);
            }
            else
            {
                await UniTask.Yield();    
            }
            
            var moveToDiscardPileSequence = DOTween.Sequence()
                .Join(cardUi.RectTransform.DOAnchorPos(Vector2.zero, duration));
            if (cardUi.RectTransform.localScale != _discardPilePanel.CardScale)
            {
                moveToDiscardPileSequence.Join(
                    cardUi.RectTransform.DOScale(_discardPilePanel.CardScale, duration));
            }
            
            if (cardUi.RectTransform.rotation != Quaternion.identity)
            {
                moveToDiscardPileSequence.Join(
                    cardUi.RectTransform.DORotate(Vector3.zero, duration));
            }
            
            await moveToDiscardPileSequence.AsyncWaitForCompletion();
            cardUi.HideShadow();
        }
        
        public UniTask DiscardAllHand()
        {
            var moveTasks = new List<UniTask>();
            
            int count = 0;
            while(_handCards.TryGetLastNotEmptySlot(out var cardUiSlot))
            {
                var cardUi = cardUiSlot.CurrentCardUi;
                cardUiSlot.RemoveCard();
                moveTasks.Add(MoveCardToDiscardPanel(cardUi, count * 200));
                ++count;
            };
            
            return UniTask.WhenAll(moveTasks);
        }

        public UniTask RemoveGoalCard(GoalCardUi cardUi)
        {
            _goalCardsPanel.RemoveGoalCard(cardUi, _freeCards);
            return MoveCardToMagicalPortal(cardUi);
        }
    }
}