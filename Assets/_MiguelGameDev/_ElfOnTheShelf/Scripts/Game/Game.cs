using Cysharp.Threading.Tasks;
using MiguelGameDev.Generic.Event;
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
        
        public async void Init(string elfName, bool useElfNameAsSeed)
        {
            if (useElfNameAsSeed)
            {
                Random.InitState(elfName.GetHashCode());
            }
            _player.InitializeDeck();
            _gameUi.Init(_player.DeckCardsLeft());
            await UniTask.Delay(1000);
            await AsyncEventDispatcherService.Instance.Dispatch(new StartGameHook());
            _turnStateMachine.Init();
        }

        public void Win()
        {
            GameUi.Instance.EndGame(true);
        }

        public void Lose()
        {
            GameUi.Instance.EndGame(false);
        }
    }
}