using Services.Player;
using Services.Time;

namespace Systems
{
    public class FlagCaptureTimeoutSystem : IUpdateSystem
    {
        private readonly ITimeProvider _timeProvider;
        private readonly PlayerHandler _playerHandler;

        public FlagCaptureTimeoutSystem(
            ITimeProvider timeProvider, 
            PlayerHandler playerHandler
        )
        {
            _timeProvider = timeProvider;
            _playerHandler = playerHandler;
        }
        
        public void Update()
        {
            foreach (var player in _playerHandler.Players)
            {
                if (player.CanCaptureFlag)
                    continue;

                var value = player.CaptureTimeout - _timeProvider.DeltaTime;
                player.ChangeCaptureTimeout(value);
                
                if (player.CaptureTimeout <=0)
                    player.ChangeCaptureAbility(true);
            }
        }
    }
}