using Core;
using Entitites;
using Factories;
using GameResult.Client;
using GameResult.Server.Impl;
using Services.FlagRepository.Impl;
using Services.FlagSpawn.Impl;
using Services.GameState.Impl;
using Services.Input.Impl;
using Services.Map;
using Services.Network;
using Services.PlayerRepository;
using Services.PlayerRepository.Impl;
using Services.QTE.Client.Impl;
using Services.QTE.Server.Impl;
using Services.Spawn.Impl;
using Services.Time.Impl;
using Systems.Client;
using Systems.Server;
using UI.Manager;
using UI.Signals;
using UI.Windows;
using UnityEngine;
using Views;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private MapView _mapView;
        [SerializeField] private NetManager _netManager;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EntryPoint>().AsSingle();
            
            BindServices();
            BindSystems();
            BindFactories();
            BindWindows();
            
            var mapHolder = new LevelSettings(_mapView);
            Container.Bind<LevelSettings>().FromInstance(mapHolder).AsSingle();
        }

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<SpawnService>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<FlagSpawnService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerSpawnService>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityTimeProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<FlagRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<ServerGameResultService>().AsSingle();
            Container.BindInterfacesAndSelfTo<QteClientService>().AsSingle();
            Container.BindInterfacesAndSelfTo<QteServerService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClientGameResultService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateService>().AsSingle();
            Container.BindInterfacesAndSelfTo<UiManager>().AsSingle();
            Container.BindInstance(_netManager);
        }

        private void BindSystems()
        {
            Container.BindInterfacesAndSelfTo<ClientSpawnPlayerSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMovementSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<FlagCaptureSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameInitializeSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<FlagCaptureTimeoutSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<StartQteSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<QteFailSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClientGameStartSystem>().AsSingle();
        }

        private void BindFactories()
        {
            Container.BindFactory<PlayerEntity, PlayerEntityFactory>().AsSingle();
            Container.BindFactory<FlagEntity, FlagEntityFactory>().AsSingle();
        }

        private void BindWindows()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<SignalOpenWindow>();
            
            Container.BindInterfacesAndSelfTo<GameHudWindow>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameResultWindow>().AsSingle();
            Container.BindInterfacesAndSelfTo<MainMenuWindow>().AsSingle();
            Container.BindInterfacesAndSelfTo<GamePendingWindow>().AsSingle();
        }
    }
}