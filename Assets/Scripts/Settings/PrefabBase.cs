using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "settings/"+ nameof(PrefabBase), fileName = nameof(PrefabBase))]
    public class PrefabBase : ScriptableObject
    {
        [SerializeField] private GameObject _playerPrefab;

        public GameObject PlayerPrefab => _playerPrefab;
    }
}