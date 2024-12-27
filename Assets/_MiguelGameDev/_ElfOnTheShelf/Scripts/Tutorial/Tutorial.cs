using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using I2.Loc;
using MiguelGameDev.Generic.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class Tutorial : SingletonBehaviour<Tutorial>
    {
        [SerializeField] private TutorialUi _tutorialUi;
        [SerializeField, TermsPopup("Tutorial/")] private string _startGameTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _firstDrawTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _actionCardsTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _deckTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _actionsRunTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _discardPileTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _goalAreaTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _goalTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _drawTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _actionsRuleTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _lastActionTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _firstGoalCompletedTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _goalShuffleTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _secondGoalCompletedTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _firstWrongSelectionTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _secondWrongSelectionTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _bustedTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _goalCardTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _magicalPortalTerm;
        [SerializeField, TermsPopup("Tutorial/")] private string _endTutorialTerm;
        
        [SerializeField] private Card[] _initialDeckOrder;
        [SerializeField] private Card[] _firstShuffleDeckOrder;
        [SerializeField] private Card[] _secondShuffleDeckOrder;

        [ShowInInspector, HideInEditorMode] private int _handDrawnCount = 0;
        [ShowInInspector, HideInEditorMode] private int _shuffleDeckCount = 0;
        
        [ShowInInspector, HideInEditorMode] private int _cardsPlayedCount = 0;
        [ShowInInspector, HideInEditorMode] private int _cardsDiscardedCount = 0;
        [ShowInInspector, HideInEditorMode] private int _bustCount = 0;
        [ShowInInspector, HideInEditorMode] private int _goalsCount = 0;
        [ShowInInspector, HideInEditorMode] private int _magicalPortalCount = 0;
        
        [ShowInInspector, HideInEditorMode] private bool _wrongSelection = false;
        
        public void Init()
        {
            var signalDispatcher = EventDispatcherService.Instance;
            var hookDispatcher = AsyncEventDispatcherService.Instance;
            
            hookDispatcher.Subscribe<StartGameHook>(OnStartGame);
            hookDispatcher.Subscribe<BeforeHandDrawHook>(OnBeforeHandDraw);
            hookDispatcher.Subscribe<AfterHandDrawnHook>(OnAfterHandDrawn);
            
            signalDispatcher.Subscribe<CardPlayedSignal>(OnCardPlayed);
            signalDispatcher.Subscribe<CardDiscardedSignal>(OnCardDiscarded);
            
            signalDispatcher.Subscribe<StartTurnSignal>(OnStartTurn);
            signalDispatcher.Subscribe<CardSelectedSignal>(OnCardSelected);
            
            hookDispatcher.Subscribe<ShuffleDeckSignal>(OnShuffleDeck);
            
            signalDispatcher.Subscribe<BustedSignal>(OnBusted);
            hookDispatcher.Subscribe<DrawCardHook>(OnDrawCard);
            hookDispatcher.Subscribe<SendDrawnCardToMagicalPortalHook>(OnSendCardToMagicalPortal);
            
            GameUi.Instance.SetOptionsButtonVisibility(false);
        }

        private async Task OnStartGame(IHook _)
        {
            Game.Instance.Player.OverrideDeck(_initialDeckOrder.Reverse().ToArray());
            await _tutorialUi.ShowDialogue(_startGameTerm, ETutorialPosition.Middle);
        }

        private async Task OnAfterHandDrawn(IHook _)
        {
            if (_handDrawnCount == 0)
            {
                _tutorialUi.SetHandOnTop(true);
                await _tutorialUi.ShowDialogue(_firstDrawTerm, ETutorialPosition.Top);

                GameUi.Instance.PlayHandHighlight();
                await _tutorialUi.ShowDialogue(_actionCardsTerm, ETutorialPosition.Top);
                GameUi.Instance.StopHandHighlight();
                
                _tutorialUi.SetHandOnTop(false);

                _tutorialUi.SetDeckOnTop(true);
                GameUi.Instance.PlayDeckHighlight();
                await _tutorialUi.ShowDialogue(_deckTerm, ETutorialPosition.Middle);
                GameUi.Instance.StopDeckHighlight();
                _tutorialUi.SetDeckOnTop(false);
                
                _tutorialUi.SetActionsRunOnTop(true);
                GameUi.Instance.PlayActionsRunHighlight();
                await _tutorialUi.ShowDialogue(_actionsRunTerm, ETutorialPosition.Middle);
                GameUi.Instance.StopActionsRunHighlight();
                _tutorialUi.SetActionsRunOnTop(false);
                
                _tutorialUi.SetDiscardPileOnTop(true);
                GameUi.Instance.PlayDiscardPileHighlight();
                await _tutorialUi.ShowDialogue(_discardPileTerm, ETutorialPosition.Middle);
                GameUi.Instance.StopDiscardPileHighlight();
                _tutorialUi.SetDiscardPileOnTop(false);
                
                _tutorialUi.SetGoalArealOnTop(true);
                GameUi.Instance.PlayGoalAreaHighlight();
                await _tutorialUi.ShowDialogue(_goalAreaTerm, ETutorialPosition.Middle);
                GameUi.Instance.StopGoalAreaHighlight();
                _tutorialUi.SetGoalArealOnTop(false);
                
                await _tutorialUi.ShowDialogue(_goalTerm, ETutorialPosition.Middle);
            }
            
            else if (_handDrawnCount == 1)
            {
                await _tutorialUi.ShowDialogue(_actionsRuleTerm, ETutorialPosition.Middle);
            }
            
            else if (_handDrawnCount == 2)
            {
                await _tutorialUi.ShowDialogue(_lastActionTerm, ETutorialPosition.Middle);
            }
            
            else if (_handDrawnCount == 3)
            {
                await _tutorialUi.ShowDialogue(_goalShuffleTerm, ETutorialPosition.Middle);
            }
            
            else if (_handDrawnCount == 9)
            {
                await _tutorialUi.ShowDialogue(_endTutorialTerm, ETutorialPosition.Middle);
                EndTutorial();
            }

            ++_handDrawnCount;
        }

        private async Task OnBeforeHandDraw(IHook _)
        {
            if (_handDrawnCount == 1)
            {
                await _tutorialUi.ShowDialogue(_drawTerm, ETutorialPosition.Middle);
                return;
            }
            
            if (_handDrawnCount == 3)
            {
                await _tutorialUi.ShowDialogue(_firstGoalCompletedTerm, ETutorialPosition.Middle);
                return;
            }

            if (_handDrawnCount == 6)
            {
                await _tutorialUi.ShowDialogue(_secondGoalCompletedTerm, ETutorialPosition.Middle);
            }
        }
        
        private void OnCardPlayed(ISignal _)
        {
            ++_cardsPlayedCount;
        }
        
        private void OnCardDiscarded(ISignal _)
        {
            ++_cardsDiscardedCount;
        }
        
        private void OnStartTurn(ISignal signal)
        {
            if (_cardsPlayedCount == 0)
            {
                GameUi.Instance.EnableAndHighlightCards(ECardSuit.Kitchen, EActionType.Prank);
                return;
            }
            
            if (_cardsPlayedCount == 1)
            {
                GameUi.Instance.EnableAndHighlightCards(ECardSuit.Kitchen, EActionType.Goodie);
                return;
            }
            
            if (_cardsPlayedCount == 2)
            {
                GameUi.Instance.EnableAndHighlightCards(ECardSuit.Kitchen, EActionType.Prank);
                return;
            }
            
            if (_cardsPlayedCount == 3)
            {
                GameUi.Instance.EnableAndHighlightCards(ECardSuit.Bathroom);
                if (_wrongSelection)
                {
                    _tutorialUi.ShowDialogue(_firstWrongSelectionTerm, ETutorialPosition.Middle);
                    _wrongSelection = false;
                }
                return;
            }
            
            if (_cardsPlayedCount == 4)
            {
                GameUi.Instance.EnableAndHighlightCards(ECardSuit.Bathroom);
                return;
            }
            
            if (_cardsPlayedCount == 5)
            {
                GameUi.Instance.EnableAndHighlightCards(ECardSuit.Bathroom);
                return;
            }
            
            if (_cardsPlayedCount == 6)
            {
                GameUi.Instance.EnableAndHighlightCards(ECardSuit.LivingRoom);
                if (_wrongSelection)
                {
                    _tutorialUi.ShowDialogue(_secondWrongSelectionTerm, ETutorialPosition.Middle);
                    _wrongSelection = false;
                }
                return;
            }
            
            if (_cardsPlayedCount == 7)
            {
                GameUi.Instance.EnableAndHighlightCards(ECardSuit.LivingRoom);
                return;
            }
            
            if (_cardsPlayedCount == 8)
            {
                GameUi.Instance.EnableAndHighlightCards(ECardSuit.LivingRoom);
                return;
            }
        }

        private async void OnCardSelected(ISignal signal)
        {
            if (_cardsPlayedCount < 9)
            {
                GameUi.Instance.SetEnableDropOnDiscardPilePanel(false);
            }
            
            if (_cardsPlayedCount == 3)
            {
                var cardSelectedSignal = (CardSelectedSignal)signal;
                var actionCardUi = (ActionCardUi)cardSelectedSignal.CardUi;
                if (actionCardUi.ActionCard.ActionType.Id == EActionType.Prank)
                {
                    await UniTask.Yield();
                    actionCardUi.CancelDrag();
                    _wrongSelection = true;
                }
            }
            
            if (_cardsPlayedCount == 6)
            {
                var cardSelectedSignal = (CardSelectedSignal)signal;
                var actionCardUi = (ActionCardUi)cardSelectedSignal.CardUi;
                if (actionCardUi.ActionCard.ActionType.Id == EActionType.Trick)
                {
                    await UniTask.Yield();
                    actionCardUi.CancelDrag();
                    _wrongSelection = true;
                }
            }
        }
        
        private async Task OnShuffleDeck(IHook _)
        {
            if (_shuffleDeckCount == 0)
            {
                Game.Instance.Player.OverrideDeck(_firstShuffleDeckOrder.Reverse().ToArray());
            }
            if (_shuffleDeckCount == 1)
            {
                Game.Instance.Player.OverrideDeck(_secondShuffleDeckOrder.Reverse().ToArray());
            }
            
            ++_shuffleDeckCount;
        }
        
        private void OnBusted(ISignal _)
        {
            if (_bustCount == 0)
            {
                GameUi.Instance.DisablePayBustWithDeck();
                GameUi.Instance.DisablePayBustWithHand();
                GameUi.Instance.DisablePayBustWithGoal();

                _tutorialUi.ShowDialogue(_bustedTerm, ETutorialPosition.Bottom);
            }
            
            ++_bustCount;
        }
        
        private async Task OnDrawCard(IHook hook)
        {
            var drawCardHook = (DrawCardHook)hook;
            if (drawCardHook.HandCardSlot.CurrentCardUi.Card.Type == ECardType.Goal)
            {
                if (_goalsCount == 0)
                {
                    await _tutorialUi.ShowDialogue(_goalCardTerm, ETutorialPosition.Middle);
                }

                ++_goalsCount;
            }
        }
        
        
        private async Task OnSendCardToMagicalPortal(IHook hook)
        {
            var drawCardHook = (SendDrawnCardToMagicalPortalHook)hook;
            if (drawCardHook.CardUi.Card.Type == ECardType.Goal)
            {
                if (_magicalPortalCount == 0)
                {
                    GameUi.Instance.PlayMagicalPortalHighlight();
                    await _tutorialUi.ShowDialogue(_magicalPortalTerm, ETutorialPosition.Middle);
                    GameUi.Instance.StopMagicalPortalHighlight();
                }

                ++_magicalPortalCount;
            }
        }

        private void EndTutorial()
        {
            var signalDispatcher = EventDispatcherService.Instance;
            var hookDispatcher = AsyncEventDispatcherService.Instance;
            
            hookDispatcher.Unsubscribe<StartGameHook>(OnStartGame);
            hookDispatcher.Unsubscribe<BeforeHandDrawHook>(OnBeforeHandDraw);
            hookDispatcher.Unsubscribe<AfterHandDrawnHook>(OnAfterHandDrawn);
            
            signalDispatcher.Unsubscribe<CardPlayedSignal>(OnCardPlayed);
            signalDispatcher.Unsubscribe<CardDiscardedSignal>(OnCardDiscarded);
            
            signalDispatcher.Unsubscribe<StartTurnSignal>(OnStartTurn);
            signalDispatcher.Unsubscribe<CardSelectedSignal>(OnCardSelected);
            
            hookDispatcher.Unsubscribe<ShuffleDeckSignal>(OnShuffleDeck);
            
            signalDispatcher.Unsubscribe<BustedSignal>(OnBusted);
            hookDispatcher.Unsubscribe<DrawCardHook>(OnDrawCard);
            hookDispatcher.Unsubscribe<SendDrawnCardToMagicalPortalHook>(OnSendCardToMagicalPortal);
            
            GameUi.Instance.SetOptionsButtonVisibility(true);
        }
    }
}