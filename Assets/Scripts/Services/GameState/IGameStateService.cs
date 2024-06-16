using System;
using GameResult;

namespace Services.GameState
{
    public interface IGameStateService
    {
        EGameState CurrentState { get; }
        event Action<EGameState> StateChanged;

        //for server side
        public void SetGameState(EGameState state);
    }
}