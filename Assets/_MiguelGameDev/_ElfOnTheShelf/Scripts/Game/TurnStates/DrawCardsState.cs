using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class DrawCardsState : TurnState
    {        
        public DrawCardsState() : base(ETurnState.DrawCards)
        {
        }

        internal override void Start(Action<ETurnState> onStateEndCallback)
        {
            base.Start(onStateEndCallback);
            _gameUi.DisableAll();

            DrawCards();
        }

        private async void DrawCards()
        {
            // draw cards one by one
            while (_game.Player.ShouldDrawCard())
            {
                if (_game.Player.IsDeckEmpty())
                {
                    _game.Lose();
                    return;
                }
                
                var card = _game.Player.DrawCard();
                // Debug.Log($"Draw {card.Type}");
                _gameUi.PlayDeckAmount(_game.Player.DeckCardsLeft());
                await _gameUi.DrawCard(card);

                if (!await CheckDrawnCard(card))
                {
                    return;
                }
            }
            
            ChangeState(ETurnState.StartTurn);
            
            
            // if there is no cards, you lose 
            // if draw no Action Card check _game.OnlyDrawActionCards
            // if true, send card and continue drawing,
            // else if Goal and have Hide Action Card ChangeState(ETurnState.Goal) and stop
            // else if Goal send to Magical Portal and continue
            // else if Busted ChangeState(ETurnState.Busted) and stop
            
            // when all card drawed (with no stop), ChangeState(ETurnState.StartTurn) 
        }

        private async UniTask<bool> CheckDrawnCard(Card card)
        {
            switch (card.Type)
            {
                case ECardType.Action:
                    _game.Player.AddCardToHand((ActionCard)card);
                    return true;
                    
                case ECardType.Goal:
                    if (_game.OnlyDrawActionCards)
                    {
                        await _gameUi.MoveCardToMagicalPortal(card);
                        return true;
                    }
                    
                    if (!_game.Player.TryGetFirstHideCardFromHand(((GoalCard)card).Suit.Id, out var actionCard))
                    {
                        await _gameUi.MoveCardToMagicalPortal(card);
                        return true;
                    }
                    
                    // TODO: Cache actionCard to highlight and use to pay
                    
                    ChangeState(ETurnState.Goal);
                    return false;
                    
                case ECardType.Bust:
                    if (_game.OnlyDrawActionCards)
                    {
                        await _gameUi.MoveCardToMagicalPortal(card);
                        return true;
                    }
                    
                    ChangeState(ETurnState.Busted);
                    return false;
            }
            
            return true;
        }
    }
}