using System;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class PayBustedWithHandState : TurnState
    {        
        public PayBustedWithHandState() : base(ETurnState.PayBustedWithHand)
        {
        }
        
        internal override void Start(Action<ETurnState> onStateEndCallback)
        {
            base.Start(onStateEndCallback);
            
            _gameUi.SetEnableDeck(false);
            _gameUi.SetHighlightRun(false);
            _gameUi.SetHighlightDiscardPile(false);
            _gameUi.SetEnableGoals(false);
            
            _gameUi.EnableAndHighlightCardSelection(false);
        }
        
        private void OnPlayerCancel()
        {
            ChangeState(ETurnState.Busted);
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