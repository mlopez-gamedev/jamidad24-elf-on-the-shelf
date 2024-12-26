using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class Player
    {
        private const int MaxHand = 5;
        private const int SpellAmount = 5;
        private const int TotalGoalCards = 8;

        private readonly CardsCatalog _cardsCatalog;
        private readonly string _name;

        [ShowInInspector] private readonly Stack<ActionCard> _run = new Stack<ActionCard>();
        [ShowInInspector] private readonly List<ActionCard> _goalRun = new List<ActionCard>();

        [ShowInInspector] private readonly List<Card> _deck = new List<Card>();
        [ShowInInspector] private readonly List<ActionCard> _hand = new List<ActionCard>();

        [ShowInInspector]
        private readonly Dictionary<EActionType, List<Card>> _discardPile = new Dictionary<EActionType, List<Card>>();

        [ShowInInspector] private readonly List<BustCard> _discardBustCards = new List<BustCard>();

        [ShowInInspector] private readonly List<Card> _magicalPortal = new List<Card>();
        [ShowInInspector] private readonly List<GoalCard> _completedGoalCards = new List<GoalCard>();

        [ShowInInspector] public bool OnlyDrawActionCards { get; set; }
        
        [ShowInInspector] public List<ActionCard> PayCards { get; set; } = new List<ActionCard>();
        public List<GoalCard> CompletedGoalCards => _completedGoalCards;
        public int HandCardsCount => _hand.Count;

        public Player(CardsCatalog cardsCatalog)
        {
            _cardsCatalog = cardsCatalog;
        }

        public void InitializeDeck()
        {
            _deck.AddRange(_cardsCatalog.ActionCards);
            _deck.Shuffle();

            var hand = _deck.Pop(MaxHand);

            _deck.AddRange(_cardsCatalog.BustCards);
            _deck.AddRange(_cardsCatalog.GoalCards);
            _deck.Shuffle();

            _deck.AddRange(hand);
        }

        public int DeckCardsLeft()
        {
            return _deck.Count;
        }

        public bool IsDeckEmpty()
        {
            return _deck.Count == 0;
        }

        public bool ShouldDrawCard()
        {
            return _hand.Count < MaxHand;
        }

        public Card DrawCard()
        {
            Assert.IsFalse(IsDeckEmpty());

            return _deck.Pop();
        }

        public bool CanPlayAction(ActionCard card)
        {
            if (!_run.TryPeek(out var lastCardPlayed))
            {
                return true;
            }

            return lastCardPlayed.ActionType != card.ActionType;
        }

        public bool TryGetFirstTrickCardFromHand(ECardSuit suit, out ActionCard actionCard)
        {
            foreach (var card in _hand)
            {
                if (card.Type == ECardType.Action && card.ActionType.Id == EActionType.Trick && card.Suit.Id == suit)
                {
                    actionCard = card;
                    return true;
                }
            }

            actionCard = null;
            return false;
        }
        
        public List<ActionCard> GetAllTrickCardsFromHand()
        {
            var trickCards = new List<ActionCard>();
            foreach (var card in _hand)
            {
                if (card.Type == ECardType.Action && card.ActionType.Id == EActionType.Trick)
                {
                    trickCards.Add(card);
                }
            }
            
            return trickCards;
        }

        public List<Card> GetSpellCards()
        {
            Assert.IsFalse(IsDeckEmpty());

            var spellCards = new List<Card>(SpellAmount);
            for (int i = 0; i < SpellAmount; i++)
            {
                if (IsDeckEmpty())
                {
                    break;
                }

                spellCards.Add(_deck.Pop());
            }

            return spellCards;
        }

        public void AddCardToHand(ActionCard card)
        {
            Assert.IsTrue(_hand.Count < MaxHand);

            _hand.Add(card);
        }

        public bool PlayCardAndCheckGoal(ActionCard card)
        {
            _hand.Remove(card);
            _run.Push(card);
            if (_goalRun.Count == 0)
            {
                _goalRun.Add(card);
                return false;
            }

            if (_goalRun[0].Suit.Id != card.Suit.Id)
            {
                _goalRun.Clear();
                _goalRun.Add(card);
                return false;
            }

            _goalRun.Add(card);
            if (_goalRun.Count < 3)
            {
                return false;
            }

            _goalRun.Clear();
            return true;
        }

        public bool TryGetGoalCardFromDeck(ECardSuit suit, out GoalCard goalCard)
        {
            foreach (var card in _deck)
            {
                if (card.Type != ECardType.Goal)
                {
                    continue;
                }

                var checkGoalCard = (GoalCard)card;
                if (checkGoalCard.Suit.Id == suit)
                {
                    _deck.Remove(card);
                    goalCard = checkGoalCard;
                    return true;
                }
            }

            goalCard = null;
            return false;
        }

        public void DiscardCard(ActionCard card)
        {
            Debug.Log($"Discard card: {card.name}");
            _hand.Remove(card);
            AddCardToDiscardPile(card);
        }

        public void DiscardHand()
        {
            foreach (var card in _hand)
            {
                AddCardToDiscardPile(card);    
            }
            _hand.Clear();
        }
        
        public Card[] DiscardTopDeckCards(int amount)
        {
            Card[] cards = new Card[amount];
            for (int i = 0; i < amount; i++)
            {
                cards[i] = _deck.Pop();
                if (cards[i].Type == ECardType.Action)
                {
                    AddCardToDiscardPile(cards[i]);
                    continue;
                }
                
                AddCardToMagicalPortal(cards[i]);
            }

            return cards;
        }
        
        public void AddCardToDiscardPile(Card card)
        {
            Assert.IsFalse(card.Type == ECardType.Goal);
            if (card.Type == ECardType.Action)
            {
                AddActionCardToDiscardPile(card as ActionCard);
            }
            else if (card.Type == ECardType.Bust)
            {
                AddBustCardToDiscardPile(card as BustCard);
            }
        }

        public void AddActionCardToDiscardPile(ActionCard card)
        {
            if (!_discardPile.ContainsKey(card.ActionType.Id))
            {
                _discardPile.Add(card.ActionType.Id, new List<Card>());
            }

            _discardPile[card.ActionType.Id].Add(card);
        }

        public void AddBustCardToDiscardPile(BustCard card)
        {
            _discardBustCards.Add(card);
        }

        public void AddCardToMagicalPortal(Card card)
        {
            _magicalPortal.Add(card);
        }

        public void RemoveCardFromMagicalPortal(Card card)
        {
            _magicalPortal.Remove(card);
        }

        public bool AddCardCompletedGoalsAndCheckVictory(GoalCard card)
        {
            _deck.Remove(card);
            _completedGoalCards.Add(card);
            return _completedGoalCards.Count == TotalGoalCards;
        }

        public bool TryGetMagicalPortalCards(out Card[] cards)
        {
            if (_magicalPortal.Count == 0)
            {
                cards = null;
                return false;
            }

            cards = _magicalPortal.ToArray();
            return true;
        }

        public void ShuffleDeck()
        {
            _deck.Shuffle();
        }

        public void AddCardToDeck(Card card)
        {
            _deck.Add(card);
        }

        public void RemoveGoal(GoalCard goalCard)
        {
            _completedGoalCards.Remove(goalCard);
        }
    }
}