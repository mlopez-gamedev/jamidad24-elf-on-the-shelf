using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class CardUiFactory : SingletonBehaviour<CardUiFactory>
    {
        [SerializeField] private ActionCardUi _actionCardUiPrefab;
        [SerializeField] private GoalCardUi _goalCardUiPrefab;
        [SerializeField] private BustCardUi _bustCardUiPrefab;

        [ShowInInspector, HideInEditorMode] private Dictionary<Card, CardUi> _cardUis = new Dictionary<Card, CardUi>();

        public CardUi GetCardUi(Card card)
        {
            return _cardUis[card];
        }
        
        public CardUi CreateCardUi(Card card, Transform parent)
        {
            switch (card.Type)
            {
                case ECardType.Action:
                    return CreateActionCardUi(card as ActionCard, parent);

                case ECardType.Goal:
                    return CreateGoalCardUi(card as GoalCard, parent);
                
                case ECardType.Bust:
                    return CreateBustCardUi(card as BustCard, parent);
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ActionCardUi CreateActionCardUi(ActionCard card, Transform parent)
        {
            var cardUi = Instantiate(_actionCardUiPrefab, parent);
            cardUi.Setup(card);
            _cardUis.Add(card, cardUi);
            return cardUi;
        }
        
        private GoalCardUi CreateGoalCardUi(GoalCard card, Transform parent)
        {
            var cardUi = Instantiate(_goalCardUiPrefab, parent);
            cardUi.Setup(card);
            _cardUis.Add(card, cardUi);
            return cardUi;
        }
        
        private BustCardUi CreateBustCardUi(BustCard card, Transform parent)
        {
            var cardUi = Instantiate(_bustCardUiPrefab, parent);
            cardUi.Setup(card);
            _cardUis.Add(card, cardUi);
            return cardUi;
        }
    }
}