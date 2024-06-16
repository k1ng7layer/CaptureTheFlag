using System;
using System.Collections.Generic;
using UI.Signals;
using Zenject;

namespace UI.Manager
{
    public class UiManager : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly Dictionary<Type, IUiWindow> _windows = new();
        private readonly Dictionary<Type, IUiController> _controllers = new();
        private IUiWindow _currentWindow;

        public UiManager(SignalBus signalBus, 
            List<IUiWindow> uiWindows, 
            List<IUiController> controllers
        )
        {
            _signalBus = signalBus;
            
            foreach (var controller in controllers)
            {
                _controllers.Add(controller.GetType(), controller);
                controller.Close();
            }
            
            foreach (var window in uiWindows)
            {
                window.Setup();
                
                _windows.Add(window.GetType(), window);
            }
        }

        public void Initialize()
        {
            _signalBus.Subscribe<SignalOpenWindow>(OpenNewWindow);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<SignalOpenWindow>(OpenNewWindow);
        }

        private void OpenNewWindow(SignalOpenWindow signal)
        {
            if (_currentWindow != null)
                CloseWindow(_currentWindow);

            var window = _windows[signal.WindowType];
            OpenWindow(window);
        }
        
        private void OpenWindow(IUiWindow uiWindow)
        {
            _currentWindow = uiWindow;
            
            foreach (var controllerType in uiWindow.Controllers)
            {
                var controller = _controllers[controllerType];
                controller.Open();
            }
        }

        private void CloseWindow(IUiWindow window)
        {
            foreach (var controllerType in window.Controllers)
            {
                var controller = _controllers[controllerType];
                controller.Close();
            }
        }
    }
}