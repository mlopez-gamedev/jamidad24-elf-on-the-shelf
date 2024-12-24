using System;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class BustedState : TurnState
    {
        public BustedState() : base(ETurnState.Busted)
        {
        }

        internal override void Start(Action<ETurnState> onStateEndCallback)
        {
            base.Start(onStateEndCallback);
            _gameUi.DisableAll();
            _gameUi.ShowBustedPanel();
        }
    }
}