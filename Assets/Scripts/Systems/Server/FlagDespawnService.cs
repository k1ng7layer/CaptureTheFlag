using System;
using Entitites;
using Services.FlagRepository;
using Zenject;

namespace Systems.Server
{
    public class FlagDespawnService : IInitializable, IDisposable
    {
        private readonly IFlagRepository _flagRepository;

        public FlagDespawnService(IFlagRepository flagRepository)
        {
            _flagRepository = flagRepository;
        }

        public void Dispose()
        {
            _flagRepository.Added -= OnFlagCreated;
        }

        public void Initialize()
        {
            _flagRepository.Added += OnFlagCreated;
        }

        private void OnFlagCreated(FlagEntity flagEntity)
        {
            _flagRepository.Added -= OnFlagCreated;
            
            flagEntity.CaptureCompleted += OnCapturedCompleted;
        }

        private void OnCapturedCompleted(FlagEntity entity)
        {
            entity.CaptureCompleted -= OnCapturedCompleted;
            
            _flagRepository.Remove(entity);
        }
    }
}