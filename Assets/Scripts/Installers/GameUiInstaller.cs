using UI.Input;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameUiInstaller : MonoInstaller
    {
        [SerializeField] private JoystickView _joystickView;
        
        public override void InstallBindings()
        {
            BindViews();
            BindControllers();
        }

        private void BindViews()
        {
            Container.BindInterfacesAndSelfTo<JoystickView>().FromInstance(_joystickView).AsSingle();
        }

        private void BindControllers()
        {
            Container.BindInterfacesAndSelfTo<InputController>().AsSingle();
        }
    }
}