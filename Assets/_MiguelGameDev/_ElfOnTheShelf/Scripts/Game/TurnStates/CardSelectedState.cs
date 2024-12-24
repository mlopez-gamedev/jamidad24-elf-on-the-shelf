using System;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class CardSelectedState : TurnState
    {
        public CardSelectedState() : base(ETurnState.CardSelected)
        {
        }
        
        internal override void Start(Action<ETurnState> onStateEndCallback)
        {
            base.Start(onStateEndCallback);
            _gameUi.SetEnableCardSelection(false);
            _gameUi.SetEnableDeck(false);
            _gameUi.SetEnableGoals(false);
            
            _gameUi.SetHighlightRun(true);
            _gameUi.SetHighlightDiscardPile(true);
        }

        private void OnDeselect()
        {
            ChangeState(ETurnState.StartTurn);
        }
        
        private void OnTryPlayCardOnRun(ActionCard card)
        {
            if (false)
            {
                ChangeState(ETurnState.StartTurn);
            }
            
            // Add Card to Run
            // Check if it completes a goal
            // if yes, add goal to goals panel
            // draw card
            // if draw a portal, check if you have, a key
        }
    }
}