using System;

namespace Services.QTE.Server
{
    public class QteDelayedStart
    {
        public readonly int ConnectionId;
        private bool _completed;
        private float _time;

        public QteDelayedStart(int connectionId, float time)
        {
            ConnectionId = connectionId;
            _time = time;
        }

        public event Action<QteDelayedStart> Elapsed;

        public void Tick(float deltaTime)
        {
            if (_completed)
                return;

            if (_time <= 0)
            {
                Elapsed?.Invoke(this);
                _completed = true;
            }
               
            
            _time -= deltaTime;
        }
    }
}