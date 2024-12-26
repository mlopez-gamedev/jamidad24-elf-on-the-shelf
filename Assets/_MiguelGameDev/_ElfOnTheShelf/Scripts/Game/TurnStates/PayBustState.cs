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
            PayBust();
        }

        private async void PayBust()
        {
            PayBustOptionWithCard payBustOptionWithCard;
            var payBustOption = await _gameUi.ShowPayBustPanel(_game.Player.PayCards.ToArray());
            switch (payBustOption.Id)
            {
                case EPayBustOption.DiscardHand:
                    _game.Player.DiscardHand();
                    _game.Player.OnlyDrawActionCards = true;
                    await GameUi.Instance.DiscardAllHand();
                    break;
                
                case EPayBustOption.DiscardTrickCard:
                    payBustOptionWithCard = (PayBustOptionWithCard)payBustOption;
                    var trickCardUi = (ActionCardUi)payBustOptionWithCard.CardUi;
                    _game.Player.DiscardCard(trickCardUi.ActionCard);
                    await GameUi.Instance.MoveCardToDiscardPanel(trickCardUi);
                    break;
                
                case EPayBustOption.DiscardDeckCards:
                    var discards = _game.Player.DiscardTopDeckCards(5);
                    await _gameUi.DiscardTopDeckCards(discards);
                    break;
                
                case EPayBustOption.RemoveGoal:
                    payBustOptionWithCard = (PayBustOptionWithCard)payBustOption;
                    var goalCardUi = (GoalCardUi)payBustOptionWithCard.CardUi;
                    _game.Player.RemoveGoal((GoalCard)goalCardUi.Card);
                    _game.Player.AddCardToMagicalPortal(goalCardUi.Card);
                    await GameUi.Instance.RemoveGoalCard(goalCardUi);
                    break;
            }
            
            _game.Player.AddCardToDiscardPile(_gameUi.DrawnBustCardUi.Card);

            _game.Player.PayCards.Clear();
            _gameUi.DrawnBustCardUi = null;
            
            ChangeState(ETurnState.DrawCards);
        }
    }
}