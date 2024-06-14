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
        [SerializeField] private FlagSettings _flagSettings;
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private QteSettings _qteSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(_prefabBase).AsSingle();
            Container.BindInstance(_playerColorSettings).AsSingle();
            Container.BindInstance(_flagSettings).AsSingle();
            Container.BindInstance(_gameSettings).AsSingle();
            Container.BindInstance(_qteSettings).AsSingle();
        }
    }
}