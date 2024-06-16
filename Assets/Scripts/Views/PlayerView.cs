using Entitites;
using Settings;

namespace Views
{
    public class PlayerView : GameEntityView
    {
        private GameEntity _entity;

        private void Update()
        {
            if (_entity == null || isOwned)  
                    return;
            
            _entity.SetPosition(transform.position);
            _entity.SetRotation(transform.rotation);
        }

        public override void Initialize(GameEntity entity)
        {
            base.Initialize(entity);
            
            _entity = entity;
        }

        protected override void SetupAsClient(GameEntity entity)
        {
            base.SetupAsClient(entity);
            
            entity.SetColor((EColor)Color);
        }
    }
}