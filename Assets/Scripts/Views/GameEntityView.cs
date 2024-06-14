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
        [SerializeField] protected MeshRenderer _meshRenderer;
        [SerializeField] protected MeshFilter _meshFilter;
        [SerializeField] protected PlayerColorSettings playerColorSettings;
        
        private GameEntity _entity;
        
        public event Action<IEntityView> ClientStarted;
        public event Action<IEntityView> AuthorityStarted;
        public event Action<IEntityView> LocalStarted;
        public Transform Transform => transform;
        public bool IsLocal => isLocalPlayer;
        protected Material _material;
        
        private void Awake()
        {
            _material = _meshRenderer.material;
            OnAwake();
        }

        public override void OnStartClient()
        {
            ClientStarted?.Invoke(this);
            
            OnClientStart();
        }

        protected virtual void OnAwake()
        { }

        protected virtual void OnClientStart()
        {
            
        }
        
        public override void OnStartLocalPlayer()
        {
            LocalStarted?.Invoke(this);
        }

        public virtual void Initialize(GameEntity entity)
        {
            _entity = entity;
             Debug.Log($"Initialize, isServer: {isServer}, local : {isClient}, isCLient {isClient}, isOwned {isOwned}, is {isClientOnly}");
            if (entity.IsLocalPlayer)
                SetupAsClient(entity);

            if (entity.IsServerObject)
                SetupAsServerObject(entity);
        }
        
        protected virtual void SetupAsClient(GameEntity entity)
        {
            entity.PositionChanged += SetPosition;
            entity.RotationChanged += SetRotation;
        }
        
        protected virtual void SetupAsServerObject(GameEntity entity)
        {
            
        }
        
        private void SetPosition(Vector3 position)
        {
            Debug.Log($"Set position");
            transform.position = position;
        }

        private void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        protected virtual void OnColorChanged(EColor value)
        { }
    }
}