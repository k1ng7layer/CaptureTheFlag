using System;
using Entitites;
using Mirror;
using Settings;
using UnityEngine;

namespace Views
{
    public abstract class GameEntityView : NetworkBehaviour, IEntityView
    { 
        [SyncVar(hook = nameof(ColorHook))]
        private int _var;
        
        [SerializeField] protected MeshRenderer _meshRenderer;
        [SerializeField] protected PlayerColorSettings playerColorSettings;

        private Material _material;
        
        public event Action<IEntityView> ClientStarted;
        public event Action<IEntityView> AuthorityStarted;
        public event Action<IEntityView> LocalStarted;
        public Transform Transform => transform;
        public bool IsLocal => isLocalPlayer;
        
        private void Awake()
        {
            _material = _meshRenderer.material;
            OnAwake();
        }
        
        protected virtual void OnAwake()
        { }

        private void ColorHook(int old, int newValue)
        {
            var color = playerColorSettings.Get((EColor)_var);
            _material.color = color;
            OnColorChanged((EColor)_var);
        }
        
        public override void OnStartClient()
        {
            ClientStarted?.Invoke(this);
            ColorHook(_var, _var);
            OnClientStart();
        }
        
        public override void OnStartLocalPlayer()
        {
            LocalStarted?.Invoke(this);
        }

        public virtual void Initialize(GameEntity entity)
        {
            if (entity.IsLocalPlayer)
                SetupAsClient(entity);

            if (entity.IsServerObject)
                SetupAsServerObject(entity);
        }
        
        private void ColorChanged(EColor value)
        {
            _var = (int)value;
            ColorHook(_var, _var);
        }
        
        [Server]
        private void SetColor(EColor color)
        {
            _var = (int)color;
        }
        
        private void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        private void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        protected virtual void OnColorChanged(EColor value)
        { }
        
        protected virtual void OnClientStart()
        { }
        
        protected virtual void SetupAsClient(GameEntity entity)
        {
            entity.PositionChanged += SetPosition;
            entity.RotationChanged += SetRotation;
        }
        
        protected virtual void SetupAsServerObject(GameEntity entity)
        {
            entity.ColorChanged += SetColor;
            ColorChanged(entity.Color);
        }
    }
}