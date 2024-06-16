﻿using System;
using Services.FlagRepository;
using Services.Player;
using Services.QTE.Server;
using Settings;
using UI.QteResult;
using Zenject;

namespace Systems.Server
{
    public class QteFailSystem : IInitializable, IDisposable
    {
        private readonly IQteServerService _qteServerService;
        private readonly IFlagRepository _flagRepository;
        private readonly PlayerRepository _playerRepository;
        private readonly QteSettings _qteSettings;

        public QteFailSystem(
            IQteServerService qteServerService, 
            IFlagRepository flagRepository,
            PlayerRepository playerRepository,
            QteSettings qteSettings
        )
        {
            _qteServerService = qteServerService;
            _flagRepository = flagRepository;
            _playerRepository = playerRepository;
            _qteSettings = qteSettings;
        }
        
        public void Initialize()
        {
            _qteServerService.QteCompleted += OnQteCompleted;
        }
        
        public void Dispose()
        {
            _qteServerService.QteCompleted -= OnQteCompleted;
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