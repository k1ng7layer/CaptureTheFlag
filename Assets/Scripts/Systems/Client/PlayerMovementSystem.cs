using GameResult;
using Services.GameState;
using Services.Input;
using Services.PlayerRepository;
using Services.PlayerRepository.Impl;
using Services.Time;
using Settings;
using UnityEngine;

namespace Systems.Client
{
    public class PlayerMovementSystem : IUpdateSystem
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IInputService _inputService;
        private readonly ITimeProvider _timeProvider;
        private readonly IGameStateService _gameStateService;
        private readonly PlayerSettings _playerSettings;

        public PlayerMovementSystem(
            IPlayerRepository playerRepository, 
            IInputService inputService,
            ITimeProvider timeProvider,
            IGameStateService gameStateService,
            PlayerSettings playerSettings
        )
        {
            _playerRepository = playerRepository;
            _inputService = inputService;
            _timeProvider = timeProvider;
            _gameStateService = gameStateService;
            _playerSettings = playerSettings;
        }
        
        public void Update()
        {
            if (_gameStateService.CurrentState != EGameState.Running)
                return;
            
            var player = _playerRepository.LocalPlayer;
            
            if (_inputService.Input.sqrMagnitude <= 0 || player == null)
                return;
            
            var input = _inputService.Input;

            var dir = new Vector3(input.x, 0f, input.z).normalized;
            var position = player.Position + dir * _playerSettings.MoveMoveSpeed * _timeProvider.DeltaTime;
            
            player.SetPosition(position);
        }
    }
}