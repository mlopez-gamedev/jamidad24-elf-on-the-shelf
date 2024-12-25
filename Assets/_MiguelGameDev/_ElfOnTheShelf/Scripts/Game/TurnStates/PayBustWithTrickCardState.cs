using System;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class PayBustWithTrickCardState : TurnState
    {       
        public PayBustWithTrickCardState() : base(ETurnState.PayBustWithTrickCard)
        {
        }

        internal override void Start(Action<ETurnState> onStateEndCallback)
        {
            base.Start(onStateEndCallback);
            
            _gameUi.SetEnableDeck(false);
            _gameUi.SetHighlightRun(false);
            _gameUi.SetEnableDropOnDiscardPilePanel(false);
            _gameUi.SetEnableGoals(false);
            
            _gameUi.EnableAndHighlightCardSelection(true);
        }
        
        private void OnPlayerCancel()
        {
            ChangeState(ETurnState.PayBust);
        }

        private void OnConfirm()
        {   
            // Send Pay Card to discard pile
            // Hide Busted Panel
            ChangeState(ETurnState.DrawCards);
        }
    }
}