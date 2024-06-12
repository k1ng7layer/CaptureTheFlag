using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "settings/"+ nameof(FlagSettings), fileName = nameof(FlagSettings))]
    public class FlagSettings : ScriptableObject
    {
        [SerializeField] private float _captureRadius = 6f;
        [SerializeField] private float _captureTime = 3f;

        public float CaptureRadius => _captureRadius;
        public float CaptureTime => _captureTime;
    }
}