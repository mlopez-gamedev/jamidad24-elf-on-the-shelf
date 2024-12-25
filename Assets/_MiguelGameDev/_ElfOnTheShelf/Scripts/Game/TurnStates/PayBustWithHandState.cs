using System;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class PayBustWithHandState : TurnState
    {        
        public PayBustWithHandState() : base(ETurnState.PayBustWithHand)
        {
        }
        
        internal override void Start(Action<ETurnState> onStateEndCallback)
        {
            base.Start(onStateEndCallback);
            
            _gameUi.SetEnableDeck(false);
            _gameUi.SetHighlightRun(false);
            _gameUi.SetEnableDropOnDiscardPilePanel(false);
            _gameUi.SetEnableGoals(false);
            
            _gameUi.EnableAndHighlightCardSelection(false);
        }
        
        private void OnPlayerCancel()
        {
            ChangeState(ETurnState.PayBust);
        }
        
        private void OnConfirm()
        {   
            // Send Hand to discard pile
            // Hide Busted Panel

            _game.OnlyDrawActionCards = true;
            ChangeState(ETurnState.DrawCards);
        }
    }
}