using Services.PlayerRepository;
using Services.Time;

namespace Systems.Server
{
    public class FlagCaptureTimeoutSystem : IUpdateSystem
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITimeProvider _timeProvider;

        public FlagCaptureTimeoutSystem(
            ITimeProvider timeProvider, 
            IPlayerRepository playerRepository
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