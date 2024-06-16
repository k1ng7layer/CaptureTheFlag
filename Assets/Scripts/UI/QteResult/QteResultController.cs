using System;
using Services.PlayerRepository;
using Services.QTE.Client;
using Settings;
using UI.Manager;
using Zenject;

namespace UI.QteResult
{
    public class QteResultController : UiController<QteResultView>, 
        IInitializable, 
        IDisposable
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IQteClientService _qteClientService;
        private readonly QteSettings _qteSettings;

        public QteResultController(
            IQteClientService qteClientService, 
            IPlayerRepository playerRepository,
            QteSettings qteSettings
        )
        {
            _qteClientService = qteClientService;
            _playerRepository = playerRepository;
            _qteSettings = qteSettings;
        }

        public void Dispose()
        {
            _qteClientService.QteCompleted -= HandleQteFail;
        }

        public void Initialize()
        {
            _qteClientService.QteFailed += HandleQteFail;
            View.Hide();
            View.DisplayText("");
        }

        private void HandleQteFail(EColor loser)
        {
            if (_playerRepository.LocalPlayer.Color != loser)
            {
                View.Show();
                View.DisplayPopupResult($"Игрок из другой команды проиграл мини игру", 
                    _qteSettings.TextPopupDuration);
            }
        }
    }
}