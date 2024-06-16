using System;
using GameResult.Client;
using Mirror;
using Services.Network.Handlers;
using Services.PlayerRepository;
using Services.Time;
using Settings;
using UI.QteResult;
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
        private readonly IPlayerRepository _playerRepository;
        private readonly IClientGameResultService _gameResultService;
        private bool _inProcess;
        private int _multiplier;
        private QteParams _currentQteParams;

        public QteClientService(
            ITimeProvider timeProvider, 
            QteSettings qteSettings,
            IPlayerRepository playerRepository,
            IClientGameResultService gameResultService
        )
        {
            _timeProvider = timeProvider;
            _qteSettings = qteSettings;
            _playerRepository = playerRepository;
            _gameResultService = gameResultService;
        }
        
        public float CurrentValue { get; private set; }
        public bool Running => _inProcess;

        public event Action<EColor> QteCompleted;
        public event Action<EColor> QteFailed;
        public event Action<QteParams> Started;
        
        public void Initialize()
        {
            NetworkClient.RegisterHandler<StartQteMessage>(OnQteBegin);
            NetworkClient.RegisterHandler<QteResultMessage>(OnQteResult);

            _gameResultService.GameCompleted += OnGameEnd;
        }
        
        public void Dispose()
        {
            NetworkClient.UnregisterHandler<StartQteMessage>();
            NetworkClient.UnregisterHandler<QteResultMessage>();
            
            _gameResultService.GameCompleted -= OnGameEnd;
        }
        
        public void Resolve()
        {
            if (!_inProcess)
                return;
            
            _inProcess = false;
            
            NetworkClient.Send(new QteResolveMessage{Value = CurrentValue});
        }

        private void OnGameEnd(EColor _)
        {
            _inProcess = false;
        }

        private void OnQteBegin(StartQteMessage msg)
        {
            _multiplier = 1;
            _inProcess = true;
            _currentQteParams = new QteParams(msg.ZoneStart, msg.ZoneWidth);
            
            Started?.Invoke(_currentQteParams);
        }

        private void OnQteResult(QteResultMessage msg)
        {
            if (_playerRepository.LocalPlayer.Color == msg.FailedTeam)
                _inProcess = false;
            
            if (msg.Result == EQteResult.Fail)
                QteFailed?.Invoke(msg.FailedTeam);
            
            if (msg.Result == EQteResult.Success)
                QteCompleted?.Invoke(msg.FailedTeam);
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