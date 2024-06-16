using System;
using Services.Player;
using Services.QTE.Client;
using Settings;
using Zenject;

namespace UI.QteResult
{
    public class QteResultController : UiController<QteResultView>, 
        IInitializable, 
        IDisposable
    {
        private readonly IQteClientService _qteClientService;
        private readonly PlayerRepository _playerRepository;

        public QteResultController(
            IQteClientService qteClientService, 
            PlayerRepository playerRepository
        )
        {
            _qteClientService = qteClientService;
            _playerRepository = playerRepository;
        }

        public void Initialize()
        {
            _qteClientService.QteFailed += HandleQteFail;
            View.gameObject.SetActive(false);
        }
        
        public void Dispose()
        {
            _qteClientService.QteCompleted -= HandleQteFail;
        }

        private void HandleQteFail(EColor loser)
        {
            if (_playerRepository.LocalPlayer.Color != loser)
            {
                View.gameObject.SetActive(true);
                View.DisplayResult($"Игрок из другой команды проиграл игру");
            }
        }
    }
}