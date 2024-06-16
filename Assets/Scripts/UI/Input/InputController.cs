using System;
using Services.Input;
using UI.Manager;
using UnityEngine;
using Zenject;

namespace UI.Input
{
    public class InputController : UiController<JoystickView>, 
        IInitializable, 
        IDisposable
    {
        private readonly IInputService _inputService;

        public InputController(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Dispose()
        {
            View.Dragged -= OnMovement;
        }

        public void Initialize()
        {
            View.Dragged += OnMovement;
        }

        private void OnMovement(Vector3 dir)
        {
            _inputService.SetInput(dir);
        }
    }
}