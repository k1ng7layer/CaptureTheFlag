using System;

namespace Services.QTE.Server
{
    public class QteSession
    {
        private float _duration;
        private bool _running;

        public QteSession(
            int connId, 
            float zoneStart, 
            float zoneWidth,
            float duration)
        {
            _duration = duration;
            ConnId = connId;
            ZoneStart = zoneStart;
            ZoneWidth = zoneWidth;
        }

        public int ConnId { get; }
        public float ZoneStart { get; }
        public float ZoneWidth { get; }

        public event Action<QteSession> Timeout;

        public void Start()
        {
            _running = true;
        }

        public void Update(float time)
        {
            if (!_running)
                return;

            _duration -= time;

            if (_duration <= 0)
                Complete();
        }

        private void Complete()
        {
            _running = false;
            
            Timeout?.Invoke(this);
        }
    }
}