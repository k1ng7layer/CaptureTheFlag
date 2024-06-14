using UI.Input;
using UI.QTE;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameUiInstaller : MonoInstaller
    {
        [SerializeField] private JoystickView _joystickView;
        [SerializeField] private QteView _qteView;
        
        public override void InstallBindings()
        {
            BindViews();
            BindControllers();
        }

        private void BindViews()
        {
            Container.BindInterfacesAndSelfTo<JoystickView>().FromInstance(_joystickView).AsSingle();
            Container.BindInterfacesAndSelfTo<QteView>().FromInstance(_qteView).AsSingle();
        }

        private void BindControllers()
        {
            Container.BindInterfacesAndSelfTo<InputController>().AsSingle();
            Container.BindInterfacesAndSelfTo<QteController>().AsSingle();
        }
    }
}