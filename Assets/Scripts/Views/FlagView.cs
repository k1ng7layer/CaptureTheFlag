using Entitites;
using Mirror;
using Settings;
using UnityEngine;

namespace Views
{
    public class FlagView : GameEntityView
    {
        [SyncVar(hook = nameof(SetRadiusHook))]
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
            SetCaptureRadius(flag.CaptureRadius);
        }

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

        private void SetCaptureRadius(float value)
        {
            _radius = value;
            _radiusSprite.size = new Vector2(value, value);
        }

        private void SetRadiusHook(float old, float newValue)
        {
            SetCaptureRadius(_radius);
        }

        [Server]
        private void OnServerRadiusChange(float value)
        {
            _radius = value;
        }

        [ClientRpc]
        private void SetCaptured(bool value)
        {
            gameObject.SetActive(!value);
        }
    }
}