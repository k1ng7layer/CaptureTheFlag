using Core;
using Services.Input.Impl;
using Services.MessageDispatcher;
using Services.Player;
using Services.Spawn;
using Systems;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EntryPoint>().AsSingle();
            
            BindServices();
            BindSystems();
        }

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<NetworkSpawnService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MirrorMessageDispatcher>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerHandler>().AsSingle();
        }

        private void BindSystems()
        {
            Container.BindInterfacesAndSelfTo<PlayerMovementSystem>().AsSingle();
        }
    }
}