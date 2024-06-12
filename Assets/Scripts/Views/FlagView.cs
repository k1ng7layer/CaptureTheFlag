using System;
using Mirror;
using Settings;
using UnityEngine;

namespace Views
{
    public class FlagView : NetworkBehaviour, IFlagView
    {
        [SyncVar(hook = nameof(SyncColor))]
        [SerializeField] private EColor _color;
        
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private PlayerColorSettings _playerColorSettings;
        [SerializeField] private SpriteRenderer _radius;
        
        public event Action<IEntityView> LocalStarted;
        public Vector3 Position => transform.position;
        public Transform Transform => transform;
        public bool IsLocal => isLocalPlayer;
        
        public override void OnStartClient()
        {
            SyncColor(_color, _color);
            LocalStarted?.Invoke(this);
        }
        
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void SetColor(EColor color)
        {
            OnSeverColorChange(color);
        }

        public void SetCaptureRadius(float value)
        {
            _radius.size = new Vector2(value, value);
        }

        [ClientRpc]
        public void SetCaptured(bool value)
        {
            gameObject.SetActive(!value);
        }
        
        private void SyncColor(EColor old, EColor newValue)
        {
            var color = _playerColorSettings.Get(newValue);
            _meshRenderer.material.color = color;
            _radius.sharedMaterial.color = color;
        }

        [Server]
        public void OnSeverColorChange(EColor color)
        {
            _color = color;
            Debug.Log($"{isLocalPlayer} {isServer}");
            SyncColor(_color, _color);
        }
    }
}