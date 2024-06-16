using Entitites;
using Mirror;
using Settings;
using UnityEngine;

namespace Views
{
    public class FlagView : GameEntityView
    {
        [SerializeField] private SpriteRenderer _radiusSprite;

        private FlagEntity _flagEntity;

        [SyncVar(hook = nameof(SetRadiusHook))]
        private float _radius;

        private Material _radiusMaterial;

        protected override void OnAwake()
        {
            _radiusMaterial = _radiusSprite.material;
        }

        protected override void OnClientStart()
        {
            _radiusSprite.gameObject.SetActive(isOwned);
        }

        protected override void SetupAsServerObject(GameEntity entity)
        {
            base.SetupAsServerObject(entity);
            _flagEntity = (FlagEntity)entity;
            _flagEntity.CaptureRadiusChanged += OnServerRadiusChange;
            _flagEntity.CaptureCompleted += Captured;
            SetCaptureRadius(_flagEntity.CaptureRadius);
        }

        protected override void OnDestroyed()
        {
            _flagEntity.CaptureRadiusChanged -= OnServerRadiusChange;
            _flagEntity.CaptureCompleted -= Captured;
            
            NetworkServer.Destroy(gameObject);
        }

        private void Captured(FlagEntity entity)
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
            _radiusSprite.size = new Vector2(value * 2, value * 2);
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