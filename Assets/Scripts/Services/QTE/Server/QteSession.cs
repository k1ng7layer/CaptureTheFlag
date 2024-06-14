using System;
using Services.Time;
using Settings;

namespace Services.QTE.Server
{
    public class QteSession
    {
        private readonly ITimeProvider _timeProvider;
        private bool _running;

        public QteSession(
            EColor playerColor, 
            float zoneStart, 
            float zoneWidth, 
            ITimeProvider timeProvider
        )
        {
            _timeProvider = timeProvider;
            PlayerColor = playerColor;
            ZoneStart = zoneStart;
            ZoneWidth = zoneWidth;
        }
        
        public EColor PlayerColor { get; }
        public float ZoneStart { get; }
        public float ZoneWidth { get; }
        public float Duration { get; set; }

        public event Action<QteSession> Completed;

        public void Start()
        {
            _running = true;
        }

        public void Update()
        {
            if (!_running)
                return;

            Duration -= _timeProvider.DeltaTime;

            if (Duration <= 0)
                Complete();
        }

        private void Complete()
        {
            _running = false;
            
            Completed?.Invoke(this);
        }
    }
}