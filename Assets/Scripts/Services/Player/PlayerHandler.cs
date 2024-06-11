using System;
using Entitites;
using Services.Input;
using Services.Spawn;
using Systems;
using Views;
using Zenject;

namespace Services.Player
{
    public class PlayerHandler : IInitializable, IDisposable
    {
        private readonly NetworkSpawnService _spawnService;

        public PlayerHandler(NetworkSpawnService spawnService)
        {
            _spawnService = spawnService;
        }
        
        public Entity LocalPlayerEntity { get; private set; }
        
        public void Initialize()
        {
            _spawnService.PlayerSpawned += OnPlayerSpawned;
        }

        public void Dispose()
        {
            _spawnService.PlayerSpawned -= OnPlayerSpawned;
        }

        private void OnPlayerSpawned(IEntityView entityView)
        {
            var entity = new Entitites.Player(entityView);

            if (entityView.IsLocal)
                LocalPlayerEntity = entity;
        }
    }
}