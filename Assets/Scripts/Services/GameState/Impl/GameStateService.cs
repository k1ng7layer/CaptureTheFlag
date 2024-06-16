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
        public EGameState CurrentState { get; private set; }

        public event Action<EGameState> StateChanged;
        
        public void Initialize()
        {
            NetworkClient.RegisterHandler<GameStateMessage>(SetGameState);
        }
        
        public void Dispose()
        {
            NetworkClient.UnregisterHandler<GameStateMessage>();
        }

        private void SetGameState(GameStateMessage msg)
        {
            CurrentState = msg.GameState;
            StateChanged?.Invoke(msg.GameState);
        }
        
        //for server side
        public void SetGameState(EGameState state)
        {
            CurrentState = state;
            NetworkServer.SendToAll(new GameStateMessage{GameState = state});
            StateChanged?.Invoke(state);
        }
    }
}