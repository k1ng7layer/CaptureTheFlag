using System;
using Services.FlagRepository;
using Services.PlayerRepository;
using Services.QTE.Server;
using Settings;
using UI.QteResult;
using Zenject;

namespace Systems.Server
{
    public class QteFailSystem : IInitializable, IDisposable
    {
        private readonly IFlagRepository _flagRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IQteServerService _qteServerService;
        private readonly QteSettings _qteSettings;

        public QteFailSystem(
            IQteServerService qteServerService, 
            IFlagRepository flagRepository,
            IPlayerRepository playerRepository,
            QteSettings qteSettings
        )
        {
            _qteServerService = qteServerService;
            _flagRepository = flagRepository;
            _playerRepository = playerRepository;
            _qteSettings = qteSettings;
        }

        public void Dispose()
        {
            _qteServerService.QteCompleted -= OnQteCompleted;
        }

        public void Initialize()
        {
            _qteServerService.QteCompleted += OnQteCompleted;
        }

        private void OnQteCompleted(ServerQteResultArgs resultArgs)
        {
            if (resultArgs.Result == EQteResult.Fail)
            {
                var player = _playerRepository.PlayerEntities[resultArgs.ConnectionId];
            
                foreach (var flagEntity in _flagRepository.Flags[player.Color])
                {
                    flagEntity.SetUnderCapture(false);
                }
            
                player.ChangeCapturingFlag(false);
                player.ChangeCaptureAbility(false);
                player.ChangeCaptureTimeout(_qteSettings.FlagCaptureTimeout);
                player.CurrentCapturingCount = 0;
            }
         
        }
    }
}