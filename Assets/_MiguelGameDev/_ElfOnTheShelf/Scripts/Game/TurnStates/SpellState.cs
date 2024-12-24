using System;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class SpellState : TurnState
    {      
        public SpellState() : base(ETurnState.Spell)
        {
        }

        internal override void Start(Action<ETurnState> onStateEndCallback)
        {
            base.Start(onStateEndCallback);
            _gameUi.DisableAll();
            _gameUi.ShowCardOrderPanel();
        }
    }
}