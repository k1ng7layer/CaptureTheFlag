using Entitites;
using Mirror;
using Services.FlagRepository;
using Services.FlagRepository.Impl;
using Services.Player;
using Services.Time;
using UnityEngine;

namespace Systems
{
    public class FlagCaptureSystem : IUpdateSystem
    {
        private readonly ITimeProvider _timeProvider;
        private readonly PlayerHandler _playerHandler;
        private readonly IFlagRepository _flagRepository;

        public FlagCaptureSystem(
            ITimeProvider timeProvider, 
            PlayerHandler playerHandler,
            IFlagRepository flagRepository
        )
        {
            _timeProvider = timeProvider;
            _playerHandler = playerHandler;
            _flagRepository = flagRepository;
        }
        
        public void Update()
        {
            foreach (var player in _playerHandler.Players)
            {
                CaptureFlags(player);
            }   
        }

        private void CaptureFlags(GameEntity player)
        {
            if (!_flagRepository.Flags.TryGetValue(player.Color, out var flags))
                return;
            
            foreach (var flag in flags)
            {
                if (flag.Captured)
                    continue;
                
                var dist2 = (flag.Position - player.Position).sqrMagnitude;
                
                Debug.Log($"Flag posit: {flag.Position}");
                Debug.Log($"dist2 {dist2}, check: {flag.CaptureRadius * flag.CaptureRadius}, color: {flag.Color}, player pos: {player.Position}" );
                
                if (dist2 >= flag.CaptureRadius * flag.CaptureRadius)
                    continue;

                var time = flag.CaptureTimeLeft - _timeProvider.DeltaTime;
                
                Debug.Log($"time FlagCaptureSystem: {time}");
                
                flag.ChangeCaptureTimeLeft(time);
                
                if (flag.CaptureTimeLeft <= 0)
                    flag.ChangeCaptured(true);
            }
        }
    }
}