using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class GameUi : SingletonBehaviour<GameUi>
    {
        [SerializeField] private CardUiFactory _cardUiFactory;
        [SerializeField] private Transform _deckCards;
        [SerializeField] private Transform _freeCards;
        [SerializeField] private HandCards _handCards;
        [SerializeField] private MagicalPortalCards _magicalPortalCards;
        [SerializeField] private DeckAmountUi _deckAmountUi;

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
            // TODO
            // highlight off on enable false
        }
        
        public void SetEnableDeck(bool enable)
        {
            // TODO
            // always highlight on off
        }
        
        public void SetHighlightDiscardPile(bool highlight)
        {
            // TODO
            // Highlight
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
            SetHighlightRun(false);
            SetHighlightDiscardPile(false);
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
            return MoveCardToHand(cardUi);
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
            await DOTween.Sequence()
                .Join(cardUi.transform.DOMove(cardSlot.transform.position, 0.5f))
                .Join(cardUi.transform.DORotate(cardSlot.transform.rotation.eulerAngles, 0.5f))
                .Join(cardUi.FlipUp())
                .AsyncWaitForCompletion();
            
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
            
            _magicalPortalCards.AddCard(cardUi);
        }
    }
}