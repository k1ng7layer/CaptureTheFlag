using System;
using Entitites;
using Mirror;
using Services.Network.Handlers;
using Settings;
using UnityEngine;

namespace Views
{
    public class PlayerView : GameEntityView
    {
        [SyncVar(hook = nameof(Hook))]
        private int _var;
        
        private GameEntity _entity;

        protected override void OnClientStart()
        {
            base.OnClientStart();
            Hook(_var, _var);
        }
        
        [Server]
        public void SetColor2(EColor color)
        {
            _var = (int)color;
        }

        private void Hook(int old, int newValue)
        {
            var color = playerColorSettings.Get((EColor)_var);
            _material.color = color;
        }
        
        public override void Initialize(GameEntity entity)
        {
            base.Initialize(entity);
            
            _entity = entity;
        }

        protected override void SetupAsServerObject(GameEntity entity)
        {
            base.SetupAsServerObject(entity);
            entity.ColorChanged += SetColor2;
            ColorChanged(entity.Color);
        }
        
        protected void ColorChanged(EColor value)
        {
            _var = (int)value;
            Hook(_var, _var);
        }

        private void Update()
        {
            if (_entity == null || isOwned)  
                    return;
            
            _entity.SetPosition(transform.position);
            _entity.SetRotation(transform.rotation);
        }
    }
}