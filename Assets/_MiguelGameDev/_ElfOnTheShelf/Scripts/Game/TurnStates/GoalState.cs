using System;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class GoalState : TurnState
    {        
        public GoalState() : base(ETurnState.Goal)
        {
        }

        internal override void Start(Action<ETurnState> onStateEndCallback)
        {
            base.Start(onStateEndCallback);
            _gameUi.DisableAll();
            _gameUi.ShowGoalPanel();
        }
    }
}