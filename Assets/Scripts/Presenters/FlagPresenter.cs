using System;
using Entitites;
using Settings;
using Views;
using Zenject;

namespace Presenters
{
    public class FlagPresenter : IInitializable, IDisposable
    {
        private readonly IFlagView _view;
        private readonly FlagEntity _flagEntity;

        public FlagPresenter(IFlagView view, FlagEntity flagEntity)
        {
            _view = view;
            _flagEntity = flagEntity;
        }

        public void Initialize()
        {
            OnFlagColorChanged(_flagEntity.Color);
            OnCapturedRadiusChanged(_flagEntity.CaptureRadius);
            
            _flagEntity.ColorChanged += OnFlagColorChanged;
            _flagEntity.CaptureRadiusChanged += OnCapturedRadiusChanged;
            _flagEntity.CaptureCompleted += OnCaptured;
        }

        public void Dispose()
        {
            _flagEntity.ColorChanged-= OnFlagColorChanged;
            _flagEntity.CaptureRadiusChanged -= OnCapturedRadiusChanged;
            _flagEntity.CaptureCompleted -= OnCaptured;
        }
        
        private void OnFlagColorChanged(EColor color)
        {
            _view.SetColor(color);
        }

        private void OnCapturedRadiusChanged(float value)
        {
            _view.SetCaptureRadius(value);
        }

        private void OnCaptured()
        {
            _view.SetCaptured(true);
        }
    }
}