using System;

namespace Services.QTE.Server
{
    public class QteDelayedStart
    {
        public readonly int ConnectionId;
        private float _time;

        public QteDelayedStart(int connectionId, float time)
        {
            ConnectionId = connectionId;
            _time = time;
        }
        
        public event Action<QteDelayedStart> Elapsed; 

        public void Tick(float deltaTime)
        {
            if (_time <= 0)
                return;
            
            _time -= deltaTime;
            
            if (_time <= 0)
                Elapsed?.Invoke(this);
        }
    }
}