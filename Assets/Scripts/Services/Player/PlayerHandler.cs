using Services.Input;
using Services.Spawn;
using Systems;
using Views;

namespace Services.Player
{
    public class PlayerHandler
    {
        private readonly NetworkSpawnService _spawnService;
        private readonly IInputService _inputService;
        private PlayerMovementSystem _playerMovementSystem;

        public PlayerHandler(NetworkSpawnService spawnService, IInputService inputService)
        {
            _spawnService = spawnService;
            _inputService = inputService;
        }

        public void Init()
        {
            _spawnService.PlayerSpawned += OnPlayerSpawned;
        }

        public void Tick()
        {
            if (_playerMovementSystem != null)
                _playerMovementSystem.Update();
        }

        private void OnPlayerSpawned(IEntityView entityView)
        {
            var entity = new Entitites.Player(entityView);

            if (entityView.IsLocal)
            {
                _playerMovementSystem = new PlayerMovementSystem(entity, _inputService);
            }
        }
    }
}