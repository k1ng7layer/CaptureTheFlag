using System;
using Entitites;
using Settings;
using UnityEngine;

namespace Views
{
    public interface IEntityView
    {
        event Action<IEntityView> ClientStarted;
        event Action<IEntityView> AuthorityStarted;
        event Action<IEntityView> LocalStarted;
        Transform Transform { get; }
        bool IsLocal { get; }
        void Initialize(GameEntity entity);
    }
}