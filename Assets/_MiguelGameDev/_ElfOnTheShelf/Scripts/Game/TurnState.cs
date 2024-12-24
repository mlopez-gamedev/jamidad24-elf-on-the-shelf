using System;

namespace MiguelGameDev.ElfOnTheShelf
{
    public abstract class TurnState
    {
        private readonly ETurnState _id;
        protected readonly Game _game = Game.Instance;
        protected readonly GameUi _gameUi = GameUi.Instance;

        private Action<ETurnState> _onStateEndCallback;
        public ETurnState Id => _id;

        protected TurnState(ETurnState id)
        {
            _id = id;
        }

        internal virtual void Start(Action<ETurnState> onStateEndCallback)
        {
            _onStateEndCallback = onStateEndCallback;
        }

        protected void ChangeState(ETurnState state)
        {
            _onStateEndCallback?.Invoke(state);
        }

        internal virtual void Stop()
        {
            _onStateEndCallback = null;
        }
    }
}
