using System;
using Services;
using Services.Input;
using UnityEngine;

namespace UI.Input
{
    public class InputController : UiController<JoystickView>, IDisposable
    {
        private readonly IInputService _inputService;

        public InputController(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Init()
        {
            View.Dragged += OnMovement;
        }
        
        public void Dispose()
        {
            View.Dragged -= OnMovement;
        }

        private void OnMovement(Vector3 dir)
        {
            _inputService.SetInput(dir);
        }
    }
}