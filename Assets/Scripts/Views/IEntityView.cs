using System;
using Entitites;
using Settings;
using UnityEngine;

namespace Views
{
    public interface IEntityView
    {
        event Action<IEntityView> ClientStarted;
        event Action<IEntityView> LocalPlayerStarted;
        Transform Transform { get; }
        bool IsLocalPlayer { get; }
        void Initialize(GameEntity entity);
    }
}