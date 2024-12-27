using System;
using MiguelGameDev.Generic.Event;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class PayGoalState : TurnState
    {        
        public PayGoalState() : base(ETurnState.PayGoal)
        {
        }

        internal override void Start(Action<ETurnState> onStateEndCallback)
        {
            base.Start(onStateEndCallback);
            _gameUi.DisableAll();
            PayGoal();
        }

        private async void PayGoal()
        {
            if (await _gameUi.ShowPayGoalPanel(_game.Player.PayCards[0]))
            {
                _game.Player.DiscardCard(_game.Player.PayCards[0]);
                if (_game.Player.AddCardCompletedGoalsAndCheckVictory((GoalCard)_gameUi.DrawnGoalCardUi.Card))
                {
                    _game.Win();
                    return;
                }
            }
            else
            {
                _game.Player.AddCardToMagicalPortal(_gameUi.DrawnGoalCardUi.Card);
            }

            _game.Player.PayCards.Clear();
            _gameUi.DrawnGoalCardUi = null;
            
            ChangeState(ETurnState.DrawCards);
        }
    }
}