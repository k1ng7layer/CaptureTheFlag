using Settings;
using UnityEngine;
using Zenject;

namespace Installers.Settings
{
    [CreateAssetMenu(menuName = "settings/Installers/"+ nameof(GameSettingsInstaller), fileName = nameof(GameSettingsInstaller))]
    public class GameSettingsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private PrefabBase _prefabBase;
        [SerializeField] private PlayerColorSettings _playerColorSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(_prefabBase).AsSingle();
            Container.BindInstance(_playerColorSettings).AsSingle();
        }
    }
}