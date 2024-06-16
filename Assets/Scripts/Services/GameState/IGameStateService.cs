using System;
using GameResult;

namespace Services.GameState
{
    public interface IGameStateService
    {
        event Action<EGameState> StateChanged;
        
        EGameState CurrentState { get; }

        //for server side
        public void SetGameState(EGameState state);
    }
}