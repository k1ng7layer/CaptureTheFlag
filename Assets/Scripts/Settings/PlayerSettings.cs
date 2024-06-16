using UnityEngine;
using UnityEngine.Serialization;

namespace Settings
{
    [CreateAssetMenu(menuName = "settings/"+ nameof(PlayerSettings), fileName = nameof(PlayerSettings))]
    public class PlayerSettings : ScriptableObject
    {
        [SerializeField] private float _moveSpeed = 3f;

        public float MoveMoveSpeed => _moveSpeed;
    }
}