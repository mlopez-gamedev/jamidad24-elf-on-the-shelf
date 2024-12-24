using System;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class StartTurnState : TurnState
    {      
        public StartTurnState() : base(ETurnState.StartTurn)
        {
        }

        internal override void Start(Action<ETurnState> onStateEndCallback)
        {
            base.Start(onStateEndCallback);
            
            _game.OnlyDrawActionCards = false;
            
            _gameUi.EnableAndHighlightCardSelection(false);
            _gameUi.SetHighlightRun(false);
            _gameUi.SetHighlightDiscardPile(true);
        }
        
        private void OnSelectCard()
        {
            ChangeState(ETurnState.CardSelected);
        }
    }
}