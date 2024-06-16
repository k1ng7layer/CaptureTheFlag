using UnityEngine;
using UnityEngine.Serialization;

namespace Settings
{
    [CreateAssetMenu(menuName = "settings/"+ nameof(QteSettings), fileName = nameof(QteSettings))]
    public class QteSettings : ScriptableObject
    {
        [SerializeField] private float _minSuccessZoneNormalizedWidth = 0.2f;
        [SerializeField] private float _maxSuccessZoneNormalizedWidth = 0.4f;
        [SerializeField] private float _sliderSpeed = 1f;
        [SerializeField] private float _flagCaptureTimeout;
        [SerializeField] private float _duration = 1.5f;

        public float MinSuccessZoneNormalizedWidth => _minSuccessZoneNormalizedWidth;
        public float MaxSuccessZoneNormalizedWidth => _maxSuccessZoneNormalizedWidth;
        public float SliderSpeed => _sliderSpeed;
        public float FlagCaptureTimeout => _flagCaptureTimeout;
        public float Duration => _duration;
    }
}