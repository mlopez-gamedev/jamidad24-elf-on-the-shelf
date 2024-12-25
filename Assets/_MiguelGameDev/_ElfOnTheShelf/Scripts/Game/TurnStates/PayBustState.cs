using System;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class PayBustState : TurnState
    {
        public PayBustState() : base(ETurnState.PayBust)
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