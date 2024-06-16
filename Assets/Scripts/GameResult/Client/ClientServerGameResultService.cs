using System;
using GameResult.Server;
using GameState;
using Mirror;
using Services.Network.Handlers;
using Settings;
using Zenject;

namespace GameResult.Client
{
    public class ClientServerGameResultService : IClientGameResultService, 
        IInitializable, 
        IDisposable
    {
        public event Action<EColor> GameCompleted;
        
        public void Initialize()
        {
            NetworkClient.RegisterHandler<GameCompleteMessage>(OnGameCompleted);
        }
        
        public void Dispose()
        {
            NetworkClient.UnregisterHandler<GameCompleteMessage>();
        }

        private void OnGameCompleted(GameCompleteMessage msg)
        {
            GameCompleted?.Invoke(msg.Winner);
        }
    }
}