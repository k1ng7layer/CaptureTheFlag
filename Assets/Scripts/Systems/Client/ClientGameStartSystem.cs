using System;
using GameResult;
using Services.GameState;
using UI.Signals;
using UI.Windows;
using Zenject;

namespace Systems.Client
{
    public class ClientGameStartSystem : IInitializable, IDisposable
    {
        private readonly IGameStateService _gameStateService;
        private readonly SignalBus _signalBus;

        public ClientGameStartSystem(IGameStateService gameStateService, SignalBus signalBus)
        {
            _gameStateService = gameStateService;
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            _signalBus.OpenWindow<MainMenuWindow>();
            _gameStateService.StateChanged += OnGameStateChanged;
        }
        
        public void Dispose()
        {
            _gameStateService.StateChanged -= OnGameStateChanged;
        }
        
        private void OnGameStateChanged(EGameState gameState)
        {
            if (gameState == EGameState.Running)
                _signalBus.OpenWindow<GameHudWindow>();
        }
    }
}