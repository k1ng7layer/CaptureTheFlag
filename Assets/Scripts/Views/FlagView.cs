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
        [SerializeField] private SpriteRenderer _radiusSprite;
        
        protected override void SetupAsServerObject(GameEntity entity)
        {
            base.SetupAsServerObject(entity);
            var flag = (FlagEntity)entity;
            flag.CaptureRadiusChanged += SetCaptureRadius;
            flag.CaptureCompleted += Captured;
        }
        
        private void Captured()
        {
            gameObject.SetActive(false);
            
            SetCaptured(true);
        }

        protected override void OnColorChanged(EColor value)
        {
            _radiusSprite.material.color = playerColorSettings.Get(value);
        }

        public void SetCaptureRadius(float value)
        {
            _radiusSprite.size = new Vector2(value, value);
        }

        [ClientRpc]
        public void SetCaptured(bool value)
        {
            gameObject.SetActive(!value);
        }
    }
}