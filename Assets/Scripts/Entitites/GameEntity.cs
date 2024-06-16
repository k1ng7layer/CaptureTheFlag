using System;
using Settings;
using UnityEngine;

namespace Entitites
{
    public class GameEntity
    {
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public EColor Color { get; private set; }
        public bool IsLocalPlayer { get; set; }
        public bool IsServerObject { get; set; }
        public int OwnerId { get; set; }
        public bool Destroyed { get; private set; }
        public event Action<Vector3> PositionChanged;
        public event Action<Quaternion> RotationChanged;
        public event Action<EColor> ColorChanged;
        public event Action LocalPlayerAdded;
        public event Action EntityDestroyed;

        public void SetPosition(Vector3 position)
        {
            Position = position;
            PositionChanged?.Invoke(position);
        }

        public void SetRotation(Quaternion rotation)
        {
            Rotation = rotation;
            RotationChanged?.Invoke(rotation);
        }

        public void SetColor(EColor color)
        {
            Color = color;
            ColorChanged?.Invoke(color);
        }

        public void SetIsLocalPlayer(bool value)
        {
            IsLocalPlayer = value;
            LocalPlayerAdded?.Invoke();
        }

        public void SetDestroyed(bool value)
        {
            Destroyed = value;
            EntityDestroyed?.Invoke();
        }
    }
}