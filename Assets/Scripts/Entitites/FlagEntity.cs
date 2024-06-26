﻿using System;

namespace Entitites
{
    public class FlagEntity : GameEntity
    {
        public float CaptureRadius { get; private set; }
        public float CaptureTimeLeft { get; private set; }
        public bool Captured { get; private set; }
        public bool UnderCapture { get;private set; }

        public event Action<float> CaptureRadiusChanged;
        public event Action<float> CaptureTimeLeftChanged;
        public event Action<FlagEntity> CaptureCompleted;

        public void SetUnderCapture(bool value)
        {
            UnderCapture = value;
        }

        public void ChangeCaptureRadius(float value)
        {
            CaptureRadius = value;
            CaptureRadiusChanged?.Invoke(CaptureRadius);
        }

        public void ChangeCaptureTimeLeft(float value)
        {
            CaptureTimeLeft = value;
            CaptureTimeLeftChanged?.Invoke(value);
        }

        public void ChangeCaptured(bool value)
        {
            Captured = value;
            CaptureCompleted?.Invoke(this);
        }
    }
}