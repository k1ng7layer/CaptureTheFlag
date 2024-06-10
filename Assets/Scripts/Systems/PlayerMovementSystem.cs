using Entitites;
using Services.Input;
using UnityEngine;

namespace Systems
{
    public class PlayerMovementSystem : IUpdateSystem
    {
        private readonly Entity _player;
        private readonly IInputService _inputService;

        public PlayerMovementSystem(Entity player, IInputService inputService)
        {
            _player = player;
            _inputService = inputService;
        }
        
        public void Update()
        {
            if (_inputService.Input.sqrMagnitude <= 0)
                return;
            
            var input = _inputService.Input;

            var dir = new Vector3(input.x, 0f, input.z).normalized;
            
            _player.SetPosition(_player.Position + dir * 3f * Time.deltaTime);
        }
    }
}