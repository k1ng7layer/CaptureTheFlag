using System;
using Mirror;
using Services.Network.Handlers;
using Services.Time;
using Settings;
using Zenject;

namespace Services.QTE.Client.Impl
{
    public class QteClientService : IQteClientService, 
        ITickable, 
        IInitializable, 
        IDisposable
    {
        private readonly ITimeProvider _timeProvider;
        private readonly QteSettings _qteSettings;
        private bool _inProcess;
        private int _multiplier;
        private QteParams _currentQteParams;

        public QteClientService(
            ITimeProvider timeProvider, 
            QteSettings qteSettings
        )
        {
            _timeProvider = timeProvider;
            _qteSettings = qteSettings;
        }
        
        public float CurrentValue { get; private set; }
        
        public event Action<QteParams> Started;
        
        
        public void Initialize()
        {
            NetworkClient.RegisterHandler<StartQteMessage>(OnQteBegin);
        }
        
        public void Dispose()
        {
            NetworkClient.UnregisterHandler<StartQteMessage>();
        }
        
        public void Resolve()
        {
            if (_inProcess)
                return;
            
            _inProcess = false;
            
            NetworkClient.Send(new QteResolveMessage{Value = CurrentValue});
        }

        private void OnQteBegin(StartQteMessage msg)
        {
            _multiplier = 1;
            _inProcess = true;
            _currentQteParams = new QteParams(msg.ZoneStart, msg.ZoneWidth);
            
            Started?.Invoke(_currentQteParams);
        }
        
        public void Tick()
        {
            if (!_inProcess)
                return;

            CurrentValue += _timeProvider.DeltaTime * _qteSettings.SliderSpeed * _multiplier;

            if (CurrentValue >= 1f)
            {
                CurrentValue = 1f;
                _multiplier *= -1;
            }

            if (CurrentValue <= 0)
            {
                CurrentValue = 0;
                _multiplier *= -1;
            }
        }
    }
}