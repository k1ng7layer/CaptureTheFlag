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

        public Vector3 Position => transform.position;
        public bool IsLocal => isLocalPlayer;
        
        public override void OnStartClient()
        {
            SyncColor(_color, _color);
        }

        private void SyncColor(EColor old, EColor newValue)
        {
            var material = _playerColorSettings.Get(newValue);
            _meshRenderer.sharedMaterial = material;
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
    }
}