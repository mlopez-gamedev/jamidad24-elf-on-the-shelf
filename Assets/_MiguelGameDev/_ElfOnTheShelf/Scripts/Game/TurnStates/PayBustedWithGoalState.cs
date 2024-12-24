using System;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class PayBustedWithGoalState : TurnState
    {        
        public PayBustedWithGoalState() : base(ETurnState.PayBustedWithGoal)
        {
        }
        internal override void Start(Action<ETurnState> onStateEndCallback)
        {
            base.Start(onStateEndCallback);
            
            _gameUi.SetEnableCardSelection(false);
            _gameUi.SetEnableDeck(false);
            _gameUi.SetHighlightRun(false);
            _gameUi.SetEnableDropOnDiscardPilePanel(false);
            
            _gameUi.SetEnableGoals(true);
        }
        
        private void OnPlayerCancel()
        {
            ChangeState(ETurnState.Busted);
        }
        
        private void OnConfirm()
        {   
            // Send Goal to Magical Portal
            // Hide Busted Panel
            // DrawCards (send busted to Magical Portal)
        }
    }
}