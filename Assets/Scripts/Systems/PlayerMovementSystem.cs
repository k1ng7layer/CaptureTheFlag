using Entitites;
using Services.Input;
using Services.Player;
using UnityEngine;

namespace Systems
{
    public class PlayerMovementSystem : IUpdateSystem
    {
        private readonly PlayerRepository _playerRepository;
        private readonly IInputService _inputService;

        public PlayerMovementSystem(
            PlayerRepository playerRepository, 
            IInputService inputService
        )
        {
            _playerRepository = playerRepository;
            _inputService = inputService;
        }
        
        public void Update()
        {
            var player = _playerRepository.LocalPlayerEntityEntity;
            
            if (_inputService.Input.sqrMagnitude <= 0 || player == null)
                return;
            
            var input = _inputService.Input;

            var dir = new Vector3(input.x, 0f, input.z).normalized;
            Debug.Log($"dir{ dir}");
            player.SetPosition(player.Position + dir * 3f * Time.deltaTime);
        }
    }
}