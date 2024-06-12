using System;
using Entitites;
using Settings;
using UnityEngine;
using Views;
using Zenject;

namespace Services.Presenters
{
    public class PlayerPresenter : IInitializable, IDisposable
    {
        private readonly GameEntity _playerGameEntity;
        private readonly IEntityView _view;

        public PlayerPresenter(GameEntity playerGameEntity, IEntityView view)
        {
            _playerGameEntity = playerGameEntity;
            _view = view;
        }
        
        public void Initialize()
        {
            _playerGameEntity.PositionChanged += ChangePosition;
            _playerGameEntity.ColorChanged += ChangeColor;
        }
        
        public void Dispose()
        {
            _playerGameEntity.PositionChanged -= ChangePosition;
            _playerGameEntity.ColorChanged -= ChangeColor;
        }

        private void ChangePosition(Vector3 position)
        {
            _view.SetPosition(position);
        }

        public void OnPropertyChanged(Vector3 value)
        {
            _view.SetPosition(value);
        }

        public void ChangeColor(EColor color)
        {
            _view.SetColor(color);
        }
    }
}