using System;
using Entitites;
using Mirror;
using Services.FlagRepository;
using Services.Network.Handlers;
using Settings;
using Zenject;

namespace GameResult.Server.Impl
{
    public class ServerGameResultService : IServerGameResultService, 
        IInitializable, 
        IDisposable
    {
        private readonly IFlagRepository _flagRepository;

        public ServerGameResultService(IFlagRepository flagRepository)
        {
            _flagRepository = flagRepository;
        }

        public event Action<EColor> GameCompleted;
        
        public void Initialize()
        {
            _flagRepository.Added += OnFlagSpawned;
        }
        
        public void Dispose()
        {
            _flagRepository.Added -= OnFlagSpawned;
        }

        private void OnFlagSpawned(FlagEntity flagEntity)
        {
            flagEntity.CaptureCompleted += OnFlagCaptured;
            
            void OnFlagCaptured(FlagEntity entity)
            {
                entity.CaptureCompleted -= OnFlagCaptured;
                
                CheckWin(entity.Color);
            }
        }

        private void CheckWin(EColor color)
        {
            var allFlagsDone = CheckAllFlagsCaptured(color);
            
            if (!allFlagsDone)
                return;
            
            GameCompleted?.Invoke(color);
            
            NetworkServer.SendToAll(new GameCompleteMessage{Winner = color});
        }

        private bool CheckAllFlagsCaptured(EColor color)
        {
            foreach (var flag in _flagRepository.Flags[color])
            {
                if (!flag.Captured)
                    return false;
            }

            return true;
        }
    }
}