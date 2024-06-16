using System;

namespace Entitites
{
    public class PlayerEntity : GameEntity
    {
        public bool CanCaptureFlag { get; private set; }

        public float CaptureTimeout { get; private set; }

        public float LastTimeEvent { get; set; }

        public bool CapturingFlag { get; private set; }
        public int CurrentCapturingCount { get; set; }
        public event Action CaptureAbilityChanged;
        public event Action<float> CaptureTimeoutChanged;
        public event Action<bool, PlayerEntity> CapturingChanged;

        public void ChangeCapturingFlag(bool value)
        {
            if (value == CapturingFlag)
                return;
            
            CapturingFlag = value;
            CapturingChanged?.Invoke(value, this);
        }

        public void ChangeCaptureAbility(bool value)
        {
            //Debug.Log($"ChangeCaptureAbility: {value}");
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