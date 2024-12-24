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
            
            _gameUi.SetEnableDropOnDiscardPilePanel(false);
            _gameUi.SetEnableDropOnRunPanel(false);
            _gameUi.EnableAndHighlightCardSelection(true);

            _gameUi.OnSelectCard += OnSelectCard;
        }
        
        private void OnSelectCard(CardUi cardUi)
        {
            ChangeState(ETurnState.CardSelected);
        }

        internal override void Stop()
        {
            base.Stop();
            _gameUi.OnSelectCard -= OnSelectCard;
        }
    }
}