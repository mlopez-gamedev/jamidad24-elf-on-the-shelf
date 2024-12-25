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
                _gameUi.PlayDeckAmount(_game.Player.DeckCardsLeft());
                var cardSlot = await _gameUi.DrawCard(card);
                if (!await CheckDrawnCard(card, cardSlot))
                {
                    return;
                }
            }

            await ReintroduceCardsFromMagicalPortal();
            
            ChangeState(ETurnState.StartTurn);
        }

        private async UniTask<bool> CheckDrawnCard(Card card, HandCardSlot cardSlot)
        {
            switch (card.Type)
            {
                case ECardType.Action:
                    _game.Player.AddCardToHand((ActionCard)card);
                    return true;
                    
                case ECardType.Goal:
                    await UniTask.Delay(100);
                    GoalCardUi goalCardUi = (GoalCardUi)cardSlot.CurrentCardUi;
                    cardSlot.RemoveCard();
                    if (_game.OnlyDrawActionCards)
                    {
                        await _gameUi.MoveCardToMagicalPortal(card);
                        return true;
                    }
                    
                    if (!_game.Player.TryGetFirstTrickCardFromHand(((GoalCard)card).Suit.Id, out var actionCard))
                    {
                        await _gameUi.MoveCardToMagicalPortal(card);
                        _game.Player.AddCardToMagicalPortal(card);
                        return true;
                    }

                    _gameUi.DrawnGoalCardUi = goalCardUi;
                    _game.Player.PayCards.Add(actionCard);
                    
                    ChangeState(ETurnState.PayGoal);
                    return false;
                    
                case ECardType.Bust:
                    await UniTask.Delay(100);
                    cardSlot.RemoveCard();
                    if (_game.OnlyDrawActionCards)
                    {
                        await _gameUi.MoveCardToMagicalPortal(card);
                        return true;
                    }
                    
                    ChangeState(ETurnState.PayBust);
                    return false;
            }
            
            return true;
        }

        private async UniTask ReintroduceCardsFromMagicalPortal()
        {
            if (!_game.Player.TryGetMagicalPortalCards(out var cards))
            {
                return;
            }
            
            foreach (var card in cards)
            {
                _game.Player.RemoveCardFromMagicalPortal(card);
                _game.Player.AddCardToDeck(card);
            }

            await _gameUi.MoveCardsFromMagicalPortalToDeck();
            
            _game.Player.ShuffleDeck();
            await _gameUi.ShuffleDeck();
        }
    }
}