﻿using System;
using Services;
using Services.Input;
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
        
        public void Initialize()
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