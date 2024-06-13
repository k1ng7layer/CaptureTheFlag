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
        private GameEntity _entity;
        
        public override void Initialize(GameEntity entity)
        {
            base.Initialize(entity);

            _entity = entity;
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