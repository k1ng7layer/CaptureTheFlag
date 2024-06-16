using Entitites;
using Services.FlagRepository;
using Services.PlayerRepository;
using Services.PlayerRepository.Impl;
using Services.QTE.Server;
using Services.QTE.Server.Impl;
using Services.Time;
using Settings;
using UI.QteResult;
using UnityEngine;

namespace Systems.Server
{
    public class FlagCaptureSystem : IUpdateSystem
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IPlayerRepository _playerRepository;
        private readonly IFlagRepository _flagRepository;
        private readonly IQteServerService _qteServerService;
        private readonly FlagSettings _flagSettings;

        public FlagCaptureSystem(
            ITimeProvider timeProvider, 
            IPlayerRepository playerRepository,
            IFlagRepository flagRepository,
            IQteServerService qteServerService,
            FlagSettings flagSettings
        )
        {
            _timeProvider = timeProvider;
            _playerRepository = playerRepository;
            _flagRepository = flagRepository;
            _qteServerService = qteServerService;
            _flagSettings = flagSettings;
        }
        
        public void Update()
        {
            foreach (var player in _playerRepository.Players)
            {
                if (!player.CanCaptureFlag)
                    continue;
                
                CaptureFlags(player);
            }   
        }

        private void CaptureFlags(PlayerEntity player)
        {
            if (!_flagRepository.Flags.TryGetValue(player.Color, out var flags))
                return;
            
            foreach (var flag in flags)
            {
                //TODO: убрать удалять из коллекции
                if (flag.Captured)
                    continue;
                
                var dist2 = (flag.Position - player.Position).sqrMagnitude;

                if (dist2 >= flag.CaptureRadius * flag.CaptureRadius)
                {
                    if (flag.UnderCapture)
                        PlayerExitFlagZone(player, flag);
                    
                    continue;
                }

                if (!flag.UnderCapture)
                    PlayerEnterFlagZone(player, flag);

                var time = flag.CaptureTimeLeft - _timeProvider.DeltaTime;
                
                flag.ChangeCaptureTimeLeft(time);

                if (flag.CaptureTimeLeft <= 0)
                    CompleteFlagCapture(player, flag);
            }
        }

        private void ResetFlag(FlagEntity entity)
        {
            entity.SetUnderCapture(false);
            entity.ChangeCaptureTimeLeft(_flagSettings.CaptureTime);
        }

        private void PlayerExitFlagZone(PlayerEntity playerEntity, FlagEntity flagEntity)
        {
            ResetFlag(flagEntity);
            playerEntity.CurrentCapturingCount--;

            if (playerEntity.CurrentCapturingCount == 0)
            {
                playerEntity.ChangeCapturingFlag(false);
                _qteServerService.StopQteSession(playerEntity.OwnerId, EQteResult.Fail);
            }
        }

        private void PlayerEnterFlagZone(
            PlayerEntity player, 
            FlagEntity flag
        )
        {
            if (!player.CapturingFlag)
                player.ChangeCapturingFlag(true);
            
            if (!flag.UnderCapture)
            {
                flag.SetUnderCapture(true);
                player.CurrentCapturingCount++;
            }
        }

        private void CompleteFlagCapture(
            PlayerEntity player, 
            FlagEntity flag
        )
        {
            flag.ChangeCaptured(true);
            player.ChangeCapturingFlag(false);
            player.CurrentCapturingCount--;
        }
    }
}