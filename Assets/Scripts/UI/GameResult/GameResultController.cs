using System;
using GameResult.Client;
using Settings;
using UI.Manager;
using Zenject;

namespace UI.GameResult
{
    public class GameResultController : UiController<GameResultView>, 
        IInitializable, 
        IDisposable
    {
        private readonly IClientGameResultService _clientGameResultService;

        public GameResultController(IClientGameResultService clientGameResultService)
        {
            _clientGameResultService = clientGameResultService;
        }

        public void Dispose()
        {
            _clientGameResultService.GameCompleted -= OnGameCompleted;
        }

        public void Initialize()
        {
            View.Hide();
            
            _clientGameResultService.GameCompleted += OnGameCompleted;
        }

        private void OnGameCompleted(EColor winner)
        {
            View.ShowResult($"Победа игрока из команды {winner}");
        }
    }
}