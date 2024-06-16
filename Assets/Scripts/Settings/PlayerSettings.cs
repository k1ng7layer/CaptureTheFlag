using UnityEngine;
using UnityEngine.Serialization;

namespace Settings
{
    public class PlayerSettings : ScriptableObject
    {
        [SerializeField] private float _moveSpeed = 3f;

        public float MoveMoveSpeed => _moveSpeed;
    }
}