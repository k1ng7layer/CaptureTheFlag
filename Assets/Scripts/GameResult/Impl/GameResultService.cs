using System;
using Entitites;
using GameState;
using Services.FlagRepository;
using Services.Player;
using Settings;
using Zenject;

namespace GameResult.Impl
{
    public class GameResultService : IGameResultService, IInitializable
    {
        private readonly IFlagRepository _flagRepository;
        private readonly PlayerHandler _playerHandler;

        public GameResultService(
            IFlagRepository flagRepository, 
            PlayerHandler playerHandler
        )
        {
            _flagRepository = flagRepository;
            _playerHandler = playerHandler;
        }
        
        public event Action<EColor> GameCompleted;
        
        public void Initialize()
        {
            _flagRepository.Added += OnFlagSpawned;
        }

        private void OnFlagSpawned(FlagEntity flagEntity)
        {
            flagEntity.CaptureCompleted += OnFlagCaptured;
            
            void OnFlagCaptured()
            {
                flagEntity.CaptureCompleted -= OnFlagCaptured;
                
                CheckWin(flagEntity.Color);
            }
        }

        private void CheckWin(EColor color)
        {
            var allFlagsDone = CheckAllFlagsCaptured(color);
            
            if (!allFlagsDone)
                return;
            
            GameCompleted?.Invoke(color);
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