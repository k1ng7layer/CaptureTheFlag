﻿using System;
using UI.Manager;
using UnityEngine;

namespace UI.Input
{
    public class JoystickView : UiView
    {
        [SerializeField] private Joystick _joystick;

        public Joystick Joystick => _joystick;

        private void Update()
        {
            // if (_joystick.Horizontal != 0f || _joystick.Vertical != 0f)
            // {
            //     Dragged?.Invoke(new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical));
            // }
            
            Dragged?.Invoke(new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical));
        }

        public event Action<Vector3> Dragged;
    }
}