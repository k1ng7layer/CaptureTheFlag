using System;
using GameResult;
using Mirror;
using Network.Handlers;
using Zenject;

namespace Services.GameState.Impl
{
    public class GameStateService : IGameStateService, 
        IInitializable, 
        IDisposable
    {
        public void Dispose()
        {
            NetworkClient.UnregisterHandler<GameStateMessage>();
        }

        public EGameState CurrentState { get; private set; }

        public event Action<EGameState> StateChanged;

        //for server side
        public void SetGameState(EGameState state)
        {
            CurrentState = state;
            NetworkServer.SendToAll(new GameStateMessage{GameState = state});
            StateChanged?.Invoke(state);
        }

        public void Initialize()
        {
            NetworkClient.RegisterHandler<GameStateMessage>(SetGameState);
        }

        private void SetGameState(GameStateMessage msg)
        {
            CurrentState = msg.GameState;
            StateChanged?.Invoke(msg.GameState);
        }
    }
}