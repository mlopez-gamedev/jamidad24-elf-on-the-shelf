using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Assertions;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class Player
    {
        private const int MaxHand = 5;
        private const int SpellAmount = 5;
        
        private readonly CardsCatalog _cardsCatalog;
        private readonly string _name;

        [ShowInInspector] private readonly Stack<Card> _run = new Stack<Card>();
        
        [ShowInInspector] private readonly Stack<Card> _deck = new Stack<Card>();
        [ShowInInspector] private readonly List<ActionCard> _hand = new List<ActionCard>();
        
        [ShowInInspector] private readonly Dictionary<EActionType, List<Card>> _discardPile = new Dictionary<EActionType, List<Card>>();
        [ShowInInspector] private readonly List<BustCard> _discardBustCards = new List<BustCard>();
        
        [ShowInInspector] private readonly List<Card> _magicalPortal = new List<Card>();
        [ShowInInspector] private readonly List<GoalCard> _completedGoalCards = new List<GoalCard>();
        
        public bool OnlyDrawActionCards { get; set; }

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

        public bool TryGetFirstHideCardFromHand(ECardSuit suit, out ActionCard actionCard)
        {
            foreach (var card in _hand)
            {
                if (card.Type == ECardType.Action && card.ActionType.Id == EActionType.Hide && card.Suit.Id == suit)
                {
                    actionCard = card;
                    return true;
                }
            }

            actionCard = null;
            return false;
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
        
        public void AddCardToRun(GoalCard card)
        {
            _run.Push(card);
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
        
        public void AddCardCompletedGoals(GoalCard card)
        {
            _completedGoalCards.Add(card);
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
    }
}