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
        protected int Color;
        
        [SerializeField] protected MeshRenderer _meshRenderer;
        [SerializeField] protected PlayerColorSettings playerColorSettings;

        private Material _material;
        private GameEntity _entity;
        
        public event Action<IEntityView> ClientStarted;
        public event Action<IEntityView> LocalPlayerStarted;
        public Transform Transform => transform;
        public bool IsLocalPlayer => isLocalPlayer;
        
        private void Awake()
        {
            _material = _meshRenderer.material;
            OnAwake();
        }
        
        protected virtual void OnAwake()
        { }

        private void ColorHook(int old, int newValue)
        {
            var color = playerColorSettings.Get((EColor)Color);
            _material.color = color;
            OnColorChanged((EColor)Color);
        }
        
        public override void OnStartClient()
        {
            ClientStarted?.Invoke(this);
            ColorHook(Color, Color);
            OnClientStart();
        }
        
        public override void OnStartLocalPlayer()
        {
            LocalPlayerStarted?.Invoke(this);
        }

        public virtual void Initialize(GameEntity entity)
        {
            _entity = entity;
            
            if (entity.IsLocalPlayer)
                SetupAsClient(entity);

            if (entity.IsServerObject)
                SetupAsServerObject(entity);
        }
        
        private void ColorChanged(EColor value)
        {
            Color = (int)value;
            ColorHook(Color, Color);
        }
        
        [Server]
        private void SetColor(EColor color)
        {
            Color = (int)color;
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