using Settings;
using UnityEngine;
using Views;

namespace Services.Spawn.Impl
{
    public class SpawnService : ISpawnService
    {
        private readonly PrefabBase _prefabBase;

        public SpawnService(PrefabBase prefabBase)
        {
            _prefabBase = prefabBase;
        }

        public IEntityView Spawn(string prefabName, Vector3 position, Quaternion rotation)
        {
            var prefab = _prefabBase.Get(prefabName);
            var obj = Object.Instantiate(prefab, position, rotation);

            return obj.GetComponent<IEntityView>();
        }

        public T Spawn<T>(string prefabName, Vector3 position, Quaternion rotation) where T : IEntityView
        {
            var prefab = _prefabBase.Get(prefabName);
            var obj = Object.Instantiate(prefab, position, rotation);

            return obj.GetComponent<T>();
        }
    }
}