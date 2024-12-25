using System;
using Cysharp.Threading.Tasks;
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
            bool canPlaySelectedCard = _game.Player.CanPlayAction((ActionCard)_gameUi.SelectedActionCardUi.Card);
            
            _gameUi.SetEnableCardSelection(false);
            _gameUi.SetEnableDeck(false);
            _gameUi.SetEnableGoals(false);
            
            _gameUi.SetEnableDropOnRunPanel(canPlaySelectedCard);
            _gameUi.SetEnableDropOnDiscardPilePanel(true);

            _gameUi.OnCancelCardSelection += OnCancelCardSelection;
            _gameUi.OnPlayerPlayCard += OnPlayerPlayCard;
            _gameUi.OnPlayerDiscardCard += OnPlayerDiscardCard;
        }

        private void OnPlayerPlayCard(ActionCardUi cardUi)
        {
            var actionCard = (ActionCard)cardUi.Card;
            if (!_game.Player.PlayCardAndCheckGoal(actionCard))
            {
                ChangeState(ETurnState.DrawCards);
                return;
            }

            if (!_game.Player.TryGetGoalCardFromDeck(actionCard.Suit.Id, out var goalCard))
            {
                ShuffleAndEndTurn();
                return;
            }

            CompleteGoalAndEndTurn(goalCard);
        }
        
        private async void CompleteGoalAndEndTurn(GoalCard goalCard)
        {
            await _gameUi.CompleteGoal(goalCard);
            if (_game.Player.AddCardCompletedGoalsAndCheckVictory(goalCard))
            {
                _game.Win();
                return;
            }
            
            ShuffleAndEndTurn();
        } 

        private async void ShuffleAndEndTurn()
        {
            _game.Player.ShuffleDeck();
            await _gameUi.ShuffleDeck();
            ChangeState(ETurnState.DrawCards);
        } 
        
        private void OnPlayerDiscardCard(ActionCardUi cardUi)
        {
            _game.Player.DiscardCard(cardUi.ActionCard);
            if (cardUi.ActionCard.ActionType.Id == EActionType.Trick)
            {
                // TODO: Play Spell
                // return
            }
            
            ChangeState(ETurnState.DrawCards);
        }
        
        private async void OnCancelCardSelection(CardUi cardUi)
        {
            await _gameUi.MoveCardToHand(cardUi);
            
            ChangeState(ETurnState.StartTurn);
        }

        internal override void Stop()
        {
            _gameUi.OnCancelCardSelection -= OnCancelCardSelection;
            _gameUi.OnPlayerPlayCard -= OnPlayerPlayCard;
            _gameUi.OnPlayerDiscardCard -= OnPlayerDiscardCard;
            base.Stop();
        }
    }
}