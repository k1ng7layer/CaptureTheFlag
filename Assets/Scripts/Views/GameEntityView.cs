using System;
using Entitites;
using Mirror;
using Settings;
using UnityEngine;
using UnityEngine.Serialization;

namespace Views
{
    public abstract class GameEntityView : NetworkBehaviour, IEntityView
    {
        [SyncVar(hook = nameof(SyncColor))]
        [SerializeField] private EColor _color;
        
        [SerializeField] protected MeshRenderer _meshRenderer;
        [SerializeField] protected PlayerColorSettings playerColorSettings;
        
        private GameEntity _entity;
        
        public event Action<IEntityView> ClientStarted;
        public event Action<IEntityView> AuthorityStarted;
        public event Action<IEntityView> LocalStarted;
        public Transform Transform => transform;
        public bool IsLocal => isLocalPlayer;
        
        public override void OnStartClient()
        {
            SyncColor(_color, _color);
            ClientStarted?.Invoke(this);
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            LocalStarted?.Invoke(this);
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            AuthorityStarted?.Invoke(this);
        }

        public virtual void Initialize(GameEntity entity)
        {
            _entity = entity;
            // Debug.Log($"Initialize, isServer: {isServer}, local : {isClient}, isCLient {isClient}, isOwned {isOwned}, is {isClientOnly}");
            if (isOwned)
                SetupAsClient(entity);
            
            if (isServer)
                SetupAsServerObject(entity);
        }
        
        protected virtual void SetupAsClient(GameEntity entity)
        {
            entity.PositionChanged += SetPosition;
            entity.RotationChanged += SetRotation;
        }
        
        protected virtual void SetupAsServerObject(GameEntity entity)
        {
            entity.ColorChanged += SetColor;
        }
        
        public void SetColor(EColor color)
        {
            OnSeverColorChange(color);
        }
        
        [Server]
        private void OnSeverColorChange(EColor color)
        {
            _color = color;
            SyncColor(_color, _color);
        }

        // [Command]
        // private void Test()
        // {
        //     OnSeverColorChange(_color);
        // }
        
        private void SetPosition(Vector3 position)
        {
            Debug.Log($"Set position");
            transform.position = position;
        }

        private void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }
        
        private void SyncColor(EColor old, EColor newValue)
        {
            Debug.Log($"Initialize, isServer: {isServer}, local : {isClient}, isCLient {isClient}, isOwned {isOwned}, is {isClientOnly}");
            Debug.Log($"[{gameObject.name}] SyncColor: {newValue}");
            var color = playerColorSettings.Get(newValue);
            _meshRenderer.material.color = color;
            OnColorChanged(newValue);
        }

        protected virtual void OnColorChanged(EColor value)
        { }
    }
}