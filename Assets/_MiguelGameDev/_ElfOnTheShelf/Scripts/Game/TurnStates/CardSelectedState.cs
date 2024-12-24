using System;
using UnityEngine;

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
            
            _gameUi.SetEnableDropOnRunPanel(true);
            _gameUi.SetEnableDropOnDiscardPilePanel(true);

            _gameUi.OnCancelCardSelection += OnCancelCardSelection;
            _gameUi.OnPlayCard += OnPlayCard;
            _gameUi.OnDiscardCard += OnDiscardCard;
        }

        private void OnPlayCard(CardUi cardUi)
        {
            Debug.Log("Play card");
        }
        
        private void OnDiscardCard(CardUi cardUi)
        {
            Debug.Log("Discard card");
        }
        
        private async void OnCancelCardSelection(CardUi cardUi)
        {
            await _gameUi.MoveCardToHand(cardUi);
            
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

        internal override void Stop()
        {
            _gameUi.OnCancelCardSelection -= OnCancelCardSelection;
            base.Stop();
        }
    }
}