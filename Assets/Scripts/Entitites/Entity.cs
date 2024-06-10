using UnityEngine;

namespace Entitites
{
    public abstract class Entity
    {
        public abstract Vector3 Position { get; }
        public abstract void SetPosition(Vector3 position);
        public abstract void SetRotation(Quaternion rotation);
    }
}