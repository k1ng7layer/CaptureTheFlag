using System;
using Settings;
using UnityEngine;

namespace Views
{
    public interface IEntityView
    {
        event Action<IEntityView> LocalStarted;
        Vector3 Position { get; }
        Transform Transform { get; }
        bool IsLocal { get; }
        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
        void SetColor(EColor color);
    }
}