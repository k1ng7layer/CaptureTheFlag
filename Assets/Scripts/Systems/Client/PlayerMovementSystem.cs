using Services.Input;
using Services.Player;
using Services.Time;
using Settings;
using UnityEngine;

namespace Systems.Client
{
    public class PlayerMovementSystem : IUpdateSystem
    {
        private readonly PlayerRepository _playerRepository;
        private readonly IInputService _inputService;
        private readonly PlayerSettings _playerSettings;
        private readonly ITimeProvider _timeProvider;

        public PlayerMovementSystem(
            PlayerRepository playerRepository, 
            IInputService inputService,
            PlayerSettings playerSettings,
            ITimeProvider timeProvider
        )
        {
            _playerRepository = playerRepository;
            _inputService = inputService;
            _playerSettings = playerSettings;
            _timeProvider = timeProvider;
        }
        
        public void Update()
        {
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