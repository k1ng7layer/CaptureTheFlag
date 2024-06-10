using UnityEngine;
using Views;

namespace Entitites
{
    public class Player : Entity
    {
        private readonly IEntityView _view;

        public Player(IEntityView view) 
        {
            _view = view;
        }

        public override Vector3 Position => _view.Position;

        public override void SetPosition(Vector3 position)
        {
            _view.SetPosition(position);
        }

        public override void SetRotation(Quaternion rotation)
        {
            _view.SetRotation(rotation);
        }
    }
}