using Core;
using Entitites;
using Factories;
using GameResult.Impl;
using Services.FlagRepository.Impl;
using Services.Flags;
using Services.Flags.Impl;
using Services.Input.Impl;
using Services.Map;
using Services.MessageDispatcher;
using Services.Player;
using Services.Spawn.Impl;
using Services.Time.Impl;
using Systems;
using UnityEngine;
using Views;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private MapView _mapView;
        
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
            Container.BindInterfacesAndSelfTo<PlayerHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<FlagSpawnService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerSpawnService>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityTimeProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<FlagRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameResultService>().AsSingle();
        }

        private void BindSystems()
        {
            //Container.BindInterfacesAndSelfTo<ServerInitializePlayerSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClientInitializePlayerSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMovementSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<FlagCaptureSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<TestSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameInitializeSystem>().AsSingle();
        }

        private void BindFactories()
        {
            Container.BindFactory<GameEntity, GameEntityFactory>().AsSingle();
            Container.BindFactory<FlagEntity, FlagEntityFactory>().AsSingle();
        }
    }
}