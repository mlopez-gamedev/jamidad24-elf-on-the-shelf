using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;
using Sequence = Unity.VisualScripting.Sequence;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class GameUi : SingletonBehaviour<GameUi>
    {
        [SerializeField] private CardUiFactory _cardUiFactory;
        [SerializeField] private Transform _deckCards;
        [SerializeField] private Transform _freeCards;
        [SerializeField] private HandCards _handCards;
        [SerializeField] private RunPanel _runPanel;
        [SerializeField] private DiscardPilePanel _discardPilePanel;
        [SerializeField] private MagicalPortalCards _magicalPortalCards;
        [SerializeField] private DeckAmountUi _deckAmountUi;

        private CardUi _selectedCardUi;
        public CardUi SelectedCardUi => _selectedCardUi;
        
        public event System.Action<CardUi> OnSelectCard;
        public event System.Action<CardUi> OnCancelCardSelection;
        public event System.Action<CardUi> OnPlayCard;
        public event System.Action<CardUi> OnDiscardCard;
        
        public void Init(int amount)
        {
            _deckAmountUi.SetAmount(amount);
        }
        
        public void PlayDeckAmount(int amount)
        {
            _deckAmountUi.PlaySetAmount(amount);
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

        public void ShowBustedPanel()
        {
            // TODO
        }

        public void ShowGoalPanel()
        {
            // TODO
        }

        [Button]
        public UniTask DrawCard(Card card)
        {
            var cardUi = _cardUiFactory.CreateCardUi(card, _deckCards);
            Audio2dService.Instance.PlayAudio(EAudioEvent.DrawSfx);
            return MoveCardToHand(cardUi);
        }

        public void SelectCard(CardUi cardUi)
        {
            _selectedCardUi = cardUi;
            
            cardUi.transform.SetParent(_freeCards, true);
            cardUi.transform.DORotate(Vector3.zero, 0.05f).SetEase(Ease.Flash);
            
            OnSelectCard?.Invoke(cardUi);
        }

        public void PlayCard(CardUi cardUi)
        {
            if (cardUi != _selectedCardUi)
            {
                return;
            }
            
            Audio2dService.Instance.PlayAudio(EAudioEvent.PlayActionSfx);
            DisableAll();
            
            _selectedCardUi = null;
            OnPlayCard?.Invoke(cardUi);
        }
        
        public void DiscardCard(CardUi cardUi)
        {
            if (cardUi != _selectedCardUi)
            {
                return;
            }
            
            Audio2dService.Instance.PlayAudio(EAudioEvent.DiscardSfx);
            DisableAll();
            
            _selectedCardUi = null;
            OnDiscardCard?.Invoke(cardUi);
        }
        
        public void CancelCardSelection(CardUi cardUi)
        {
            if (cardUi != _selectedCardUi)
            {
                return;
            }

            DisableAll();
            
            _selectedCardUi = null;
            OnCancelCardSelection?.Invoke(cardUi);
        }

        public UniTask MoveCardToHand(Card card)
        {
            var cardUi = _cardUiFactory.GetCardUi(card);
            return MoveCardToHand(cardUi);
        }
        
        [Button]
        public async UniTask MoveCardToHand(CardUi cardUi)
        {
            if (!_handCards.TryGetFirstEmptySlot(out var cardSlot))
            {
                return;
            }
            
            cardUi.transform.SetParent(_freeCards, true);
            
            var duration = Mathf.Clamp(
                Vector3.Distance(cardUi.transform.position, cardSlot.transform.position) / 600f,
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
        }

        public UniTask MoveCardToMagicalPortal(Card card)
        {
            var cardUi = _cardUiFactory.GetCardUi(card);
            return MoveCardToMagicalPortal(cardUi);
        }
        
        [Button]
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
    }
}