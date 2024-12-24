using Sirenix.OdinInspector;
using UnityEngine;

namespace MiguelGameDev.ElfOnTheShelf
{
    public class Game : SingletonBehaviour<Game>
    {
        [ShowInInspector, HideInEditorMode] private Player _player;
        private TurnStateMachine _turnStateMachine;
        private GameUi _gameUi;

        public Player Player => _player;
       
        public bool OnlyDrawActionCards
        {
            get => _player.OnlyDrawActionCards;
            set => _player.OnlyDrawActionCards = value;
        }
        
        public void Setup(Player player, TurnStateMachine turnStateMachine, GameUi gameUi)
        {
            _player = player;
            _turnStateMachine = turnStateMachine;
            _gameUi = gameUi;
        }
        
        public void Init(string elfName, bool useElfNameAsSeed)
        {
            if (useElfNameAsSeed)
            {
                Random.InitState(elfName.GetHashCode());
            }
            _player.InitializeDeck();
            _gameUi.Init(_player.DeckCardsLeft());
            _turnStateMachine.Init();
        }
        
        public void Win()
        {

        }

        public void Lose()
        {

        }
    }
}