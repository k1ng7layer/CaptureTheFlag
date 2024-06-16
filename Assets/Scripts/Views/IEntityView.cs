using System;
using Entitites;
using UnityEngine;

namespace Views
{
    public interface IEntityView
    {
        Transform Transform { get; }
        bool IsLocalPlayer { get; }
        event Action<IEntityView> ClientStarted;
        event Action<IEntityView> LocalPlayerStarted;
        void Initialize(GameEntity entity);
    }
}