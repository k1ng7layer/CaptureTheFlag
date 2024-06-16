using System;
using GameResult.Server;
using Mirror;
using Services.Network.Handlers;
using Settings;
using UI.GameResult;
using UI.Signals;
using UI.Windows;
using Zenject;

namespace GameResult.Client
{
    public class ClientGameResultService : IClientGameResultService, 
        IInitializable, 
        IDisposable
    {
        private readonly SignalBus _signalBus;

        public ClientGameResultService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
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
            
            _signalBus.OpenWindow<GameResultWindow>();
        }
    }
}