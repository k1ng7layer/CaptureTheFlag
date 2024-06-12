using System;
using Mirror;
using Settings;
using UnityEngine;

namespace Views
{
    public class PlayerView : NetworkBehaviour, IEntityView
    {
        [SyncVar(hook = nameof(SyncColor))]
        [SerializeField] private EColor _color;

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private PlayerColorSettings _playerColorSettings;

        public event Action<IEntityView> LocalStarted;
        public Vector3 Position => transform.position;
        public Transform Transform => transform;
        public bool IsLocal => isLocalPlayer;
        
        public override void OnStartClient()
        {
            SyncColor(_color, _color);
            LocalStarted?.Invoke(this);
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            
            Debug.Log($"OnStartLocalPlayer");
        }
        
        private void SyncColor(EColor old, EColor newValue)
        {
            var color = _playerColorSettings.Get(newValue);
            _meshRenderer.material.color = color;
        }

        [Server]
        public void OnSeverColorChange(EColor color)
        {
            _color = color;
            Debug.Log($"{isLocalPlayer} {isServer}");
            SyncColor(_color, _color);
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
    }
}