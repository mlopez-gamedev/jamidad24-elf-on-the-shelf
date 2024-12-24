using System;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class PayBustedWithDeckState : TurnState
    {        
        public PayBustedWithDeckState() : base(ETurnState.PayBustedWithDeck)
        {
        }

        internal override void Start(Action<ETurnState> onStateEndCallback)
        {
            base.Start(onStateEndCallback);
            _gameUi.SetEnableCardSelection(false);
            _gameUi.SetHighlightRun(false);
            _gameUi.SetEnableDropOnDiscardPilePanel(false);
            _gameUi.SetEnableGoals(false);
            
            _gameUi.SetEnableDeck(true);
        }

        private void OnPlayerCancel()
        {
            ChangeState(ETurnState.Busted);
        }
        
        private void OnConfirm()
        {   
            // Send 5 top cards from Deck to discard pile
            // Hide Busted Panel
            // DrawCards (send busted to Magical Portal)
        }
    }
}