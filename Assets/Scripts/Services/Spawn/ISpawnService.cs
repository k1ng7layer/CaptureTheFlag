using UnityEngine;
using Views;

namespace Services.Spawn
{
    public interface ISpawnService
    {
        IEntityView Spawn(string prefabName, Vector3 position, Quaternion rotation);
        T Spawn<T>(string prefabName, Vector3 position, Quaternion rotation) where T : IEntityView;
    }
}