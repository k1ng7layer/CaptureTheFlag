using UI.GamePending;
using UI.GameResult;
using UI.Input;
using UI.MainMenu;
using UI.QTE;
using UI.QteResult;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameUiInstaller : MonoInstaller
    {
        [SerializeField] private JoystickView _joystickView;
        [SerializeField] private QteView _qteView;
        [SerializeField] private QteResultView _qteResultView;
        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private GameResultView _gameResultView;
        [SerializeField] private GamePendingView _gamePendingView;
        
        public override void InstallBindings()
        {
            BindViews();
            BindControllers();
        }

        private void BindViews()
        {
            Container.BindInterfacesAndSelfTo<JoystickView>().FromInstance(_joystickView).AsSingle();
            Container.BindInterfacesAndSelfTo<QteView>().FromInstance(_qteView).AsSingle();
            Container.BindInterfacesAndSelfTo<QteResultView>().FromInstance(_qteResultView).AsSingle();
            Container.BindInterfacesAndSelfTo<MainMenuView>().FromInstance(_mainMenuView).AsSingle();
            Container.BindInterfacesAndSelfTo<GameResultView>().FromInstance(_gameResultView).AsSingle();
            Container.BindInterfacesAndSelfTo<GamePendingView>().FromInstance(_gamePendingView).AsSingle();
        }

        private void BindControllers()
        {
            Container.BindInterfacesAndSelfTo<InputController>().AsSingle();
            Container.BindInterfacesAndSelfTo<QteController>().AsSingle();
            Container.BindInterfacesAndSelfTo<QteResultController>().AsSingle();
            Container.BindInterfacesAndSelfTo<MainMenuController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameResultController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GamePendingController>().AsSingle();
        }
    }
}