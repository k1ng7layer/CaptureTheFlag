using Core;
using Entitites;
using Factories;
using GameResult.Client;
using GameResult.Server.Impl;
using Services.FlagRepository.Impl;
using Services.FlagSpawn.Impl;
using Services.Input.Impl;
using Services.Map;
using Services.MessageDispatcher;
using Services.Network;
using Services.Player;
using Services.QTE.Client.Impl;
using Services.QTE.Server;
using Services.QTE.Server.Impl;
using Services.Spawn.Impl;
using Services.Time.Impl;
using Systems;
using Systems.Client;
using Systems.Server;
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
            var mapHolder = new MapSettings(_mapView);
            Container.BindInterfacesAndSelfTo<EntryPoint>().AsSingle();
            Container.Bind<MapSettings>().FromInstance(mapHolder).AsSingle();
            
            BindServices();
            BindSystems();
            BindFactories();
        }

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<SpawnService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MirrorMessageDispatcher>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<FlagSpawnService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerSpawnService>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityTimeProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<FlagRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<ServerGameResultService>().AsSingle();
            Container.BindInterfacesAndSelfTo<QteClientService>().AsSingle();
            Container.BindInterfacesAndSelfTo<QteServerService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClientServerGameResultService>().AsSingle();
            Container.BindInstance(_netManager);
        }

        private void BindSystems()
        {
            Container.BindInterfacesAndSelfTo<ClientInitializePlayerSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMovementSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<FlagCaptureSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameInitializeSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<FlagCaptureTimeoutSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<StartQteSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<QteFailSystem>().AsSingle();
        }

        private void BindFactories()
        {
            Container.BindFactory<PlayerEntity, PlayerEntityFactory>().AsSingle();
            Container.BindFactory<FlagEntity, FlagEntityFactory>().AsSingle();
            Container.BindFactory<int, float, float, float, QteSession, QteSessionFactory>().AsSingle();
        }
    }
}