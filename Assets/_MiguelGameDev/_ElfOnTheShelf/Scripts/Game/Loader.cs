using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private CardsCatalog _cardsCatalog;
        [SerializeField] private Game _game;
        [SerializeField] private GameUi _gameUi;
        
        private void Start()
        {
            Load();
        }

        private void Load()
        {
            var player = new Player(_cardsCatalog);
            
            var turnStateMachine = new TurnStateMachine(new TurnState[]
            {
                new StartTurnState(),
                new CardSelectedState(),
                new PayGoalState(),
                new PayBustState(),
                new PayBustWithHandState(),
                new PayBustWithDeckState(),
                new PayBustWithTrickCardState(), 
                new PayBustWithGoalState(),
                new DrawCardsState(),
            }, ETurnState.DrawCards);
            
            _game.Setup(player, turnStateMachine, _gameUi);
        }
    }
}