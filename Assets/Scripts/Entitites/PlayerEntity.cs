using System;

namespace Entitites
{
    public class PlayerEntity : GameEntity
    {
        public bool CanCaptureFlag { get; private set; }
        public event Action CaptureAbilityChanged;
        
        public float CaptureTimeout { get; private set; }
        public event Action<float> CaptureTimeoutChanged;
        
        public float LastTimeEvent { get; set; }

        public void ChangeCaptureAbility(bool value)
        {
            CanCaptureFlag = value;
            CaptureAbilityChanged?.Invoke();
        }

        public void ChangeCaptureTimeout(float value)
        {
            CaptureTimeout = value;
            CaptureTimeoutChanged?.Invoke(value);
        }
    }
}