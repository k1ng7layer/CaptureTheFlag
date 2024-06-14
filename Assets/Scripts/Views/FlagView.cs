using System;
using Entitites;
using Mirror;
using Settings;
using UnityEngine;
using UnityEngine.Serialization;

namespace Views
{
    public class FlagView : GameEntityView
    {
        [SyncVar(hook = nameof(SetRadius))]
        private float _radius;
        
        [SerializeField] private SpriteRenderer _radiusSprite;
        private Material _radiusMaterial;
        
        protected override void OnAwake()
        {
            _radiusMaterial = _radiusSprite.material;
        }

        protected override void SetupAsServerObject(GameEntity entity)
        {
            base.SetupAsServerObject(entity);
            var flag = (FlagEntity)entity;
            flag.CaptureRadiusChanged += OnServerRadiusChange;
            flag.CaptureCompleted += Captured;
        }

        // protected override void OnClientStart()
        // {
        //     base.OnClientStart();
        //     SetRadius(_radius, _radius);
        // }

        private void Captured()
        {
            gameObject.SetActive(false);
            
            SetCaptured(true);
        }

        protected override void OnColorChanged(EColor value)
        {
            base.OnColorChanged(value);
            
            _radiusMaterial.color = playerColorSettings.Get(value);
        }

        public void SetCaptureRadius(float value)
        {
            _radiusSprite.size = new Vector2(value, value);
        }

        private void SetRadius(float old, float newValue)
        {
            SetCaptureRadius(newValue);
        }

        [Server]
        private void OnServerRadiusChange(float value)
        {
            _radius = value;
            SetRadius(value, value);
        }

        [ClientRpc]
        public void SetCaptured(bool value)
        {
            gameObject.SetActive(!value);
        }
    }
}