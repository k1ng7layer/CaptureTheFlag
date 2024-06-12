using Mirror;
using Services.Flags;
using Services.Player;
using Services.Time;
using UnityEngine;

namespace Systems
{
    public class FlagCaptureSystem : IUpdateSystem
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IFlagsService _flagsService;
        private readonly PlayerHandler _playerHandler;

        public FlagCaptureSystem(
            ITimeProvider timeProvider,
            IFlagsService flagsService, 
            PlayerHandler playerHandler
        )
        {
            _timeProvider = timeProvider;
            _flagsService = flagsService;
            _playerHandler = playerHandler;
        }
        
        public void Update()
        {
            if (!NetworkServer.activeHost)
                return;
            
            foreach (var player in _playerHandler.Players)
            {
                var flag = _flagsService.Flags[player.Color];

                if (flag.Captured)
                    continue;
                
                var dist2 = (flag.Position - player.Position).sqrMagnitude;
                
                Debug.Log($"dist2 {dist2}, check: {flag.CaptureRadius * flag.CaptureRadius}" );
                
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