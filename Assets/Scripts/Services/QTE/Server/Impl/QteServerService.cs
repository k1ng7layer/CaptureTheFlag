using System;
using System.Collections.Generic;
using GameResult.Server;
using Mirror;
using Services.Network.Handlers;
using Services.PlayerRepository;
using Services.Time;
using Settings;
using UI.QteResult;
using Zenject;
using Random = UnityEngine.Random;

namespace Services.QTE.Server.Impl
{
    public class QteServerService : IQteServerService, 
        IInitializable, 
        IDisposable,
        ITickable
    {
        private readonly Dictionary<int, QteSession> _activeQteSessions = new();
        private readonly List<QteSession> _inactive = new();
        private readonly IPlayerRepository _playerRepository;
        private readonly QteSettings _qteSettings;
        private readonly IServerGameResultService _serverGameResultService;
        private readonly ITimeProvider _timeProvider;

        public QteServerService(
            QteSettings qteSettings,
            ITimeProvider timeProvider,
            IPlayerRepository playerRepository,
            IServerGameResultService serverGameResultService
        )
        {
            _qteSettings = qteSettings;
            _timeProvider = timeProvider;
            _playerRepository = playerRepository;
            _serverGameResultService = serverGameResultService;
        }

        public void Dispose()
        {
            NetworkServer.UnregisterHandler<QteResolveMessage>();
            _serverGameResultService.GameCompleted -= StopAllQte;
        }

        public void Initialize()
        {
            NetworkServer.RegisterHandler<QteResolveMessage>(OnQteResolve);
            _serverGameResultService.GameCompleted += StopAllQte;
        }

        public event Action<ServerQteResultArgs> QteCompleted;

        public void StartQteSession(int connectionId)
        {
            if (!NetworkServer.connections.TryGetValue(connectionId, out var conn))
                return;
            
            var zoneWidth = Random.Range(_qteSettings.MinSuccessZoneNormalizedWidth, 
                _qteSettings.MaxSuccessZoneNormalizedWidth);

            var zoneStart = Random.Range(0f, 1f - zoneWidth);
            
            conn.Send(new StartQteMessage
            {
                ZoneStart = zoneStart,
                ZoneWidth = zoneWidth
            });

            var session = new QteSession(connectionId, zoneStart, zoneWidth, _qteSettings.Duration);
            session.Timeout += OnSessionTimeout;
            session.Start();
            
            _activeQteSessions.Add(connectionId, session);
        }

        public void StopQteSession(int connectionId, EQteResult result = EQteResult.None)
        {
            if (!_activeQteSessions.ContainsKey(connectionId))
                return;

            var session = _activeQteSessions[connectionId];

            Fail(session.ConnId);
            
            _activeQteSessions.Remove(connectionId);
            _inactive.Remove(session);
        }

        public void Tick()
        {
            foreach (var sessionEntry in _activeQteSessions)
            {
                sessionEntry.Value.Update(_timeProvider.DeltaTime);
            }

            foreach (var session in _inactive)
            {
                _activeQteSessions.Remove(session.ConnId);
            }
            
            if (_inactive.Count > 0)
                _inactive.Clear();
        }

        private void StopAllQte(EColor _)
        {
            _activeQteSessions.Clear();
            _inactive.Clear();
        }

        private void OnQteResolve(
            NetworkConnectionToClient conn, 
            QteResolveMessage msg
        )
        {
            if (!_activeQteSessions.ContainsKey(conn.connectionId))
                return;
            
            var qteSession = _activeQteSessions[conn.connectionId];
            _activeQteSessions.Remove(conn.connectionId);
            
            if (IsSuccess(msg.Value, qteSession.ZoneStart, qteSession.ZoneWidth))
                Success(qteSession.ConnId);
            else Fail(qteSession.ConnId);
        }

        private bool IsSuccess(
            float selectedValue, 
            float start, 
            float width
        )
        {
            return (selectedValue >= start && selectedValue <= start + width);
        }

        private void Success(int connectionId)
        {
            QteCompleted?.Invoke(new ServerQteResultArgs(connectionId, EQteResult.Success));
        }

        private void Fail(int connectionId)
        {
            var player = _playerRepository.PlayerEntities[connectionId];
            
            NetworkServer.SendToAll(new QteResultMessage
            {
                FailedTeam = player.Color,
                Result = EQteResult.Fail
            });
            
            QteCompleted?.Invoke(new ServerQteResultArgs(connectionId, EQteResult.Fail));
        }

        private void OnSessionTimeout(QteSession session)
        {
            _inactive.Add(session);
            session.Timeout -= OnSessionTimeout;

            Fail(session.ConnId);
        }
    }
}