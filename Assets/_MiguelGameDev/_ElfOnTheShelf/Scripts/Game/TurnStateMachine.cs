using System.Collections.Generic;

namespace MiguelGameDev.ElfOnTheShelf
{

    public class TurnStateMachine
    {
        private readonly ETurnState _initState;
        private readonly Dictionary<ETurnState, TurnState> _idToState;

        private TurnState _currentState;

        public TurnStateMachine(TurnState[] turnStates, ETurnState initState)
        {
            _initState = initState;

            _idToState = new Dictionary<ETurnState, TurnState>(turnStates.Length);
            foreach (var turnState in turnStates)
            {
                _idToState.Add(turnState.Id, turnState);
            }
        }

        public void Init()
        {
            _currentState = GetState(_initState);
            _currentState.Start(ChangeState);
        }

        public void Reset()
        {
            ChangeState(ETurnState.StartTurn);
        }

        private void ChangeState(ETurnState nextState)
        {
            _currentState.Stop();
            _currentState = GetState(nextState);
            _currentState.Start(ChangeState);
        }

        private TurnState GetState(ETurnState gameState)
        {
            return _idToState[gameState];
        }

        public void Release()
        {
            _currentState?.Stop();
        }
    }
}
