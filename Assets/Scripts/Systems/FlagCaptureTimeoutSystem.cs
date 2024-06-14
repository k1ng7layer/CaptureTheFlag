using Services.Player;
using Services.Time;

namespace Systems
{
    public class FlagCaptureTimeoutSystem : IUpdateSystem
    {
        private readonly ITimeProvider _timeProvider;
        private readonly PlayerRepository _playerRepository;

        public FlagCaptureTimeoutSystem(
            ITimeProvider timeProvider, 
            PlayerRepository playerRepository
        )
        {
            _timeProvider = timeProvider;
            _playerRepository = playerRepository;
        }
        
        public void Update()
        {
            foreach (var player in _playerRepository.Players)
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