using System;
using System.Collections.Generic;
using Mirror;
using Services.Network.Handlers;
using Services.Player;
using Services.QTE.Client;
using Services.Time;
using Settings;
using Zenject;

namespace Services.QTE.Server.Impl
{
    public class QteServerService : IQteServerService, 
        IInitializable, 
        IDisposable,
        ITickable
    {
        private readonly QteSettings _qteSettings;
        private readonly PlayerRepository _playerRepository;
        private readonly ITimeProvider _timeProvider;
        private readonly Dictionary<int, QteSession> _activeQteSessions = new();

        public QteServerService(
            QteSettings qteSettings, 
            PlayerRepository playerRepository, 
            ITimeProvider timeProvider
        )
        {
            _qteSettings = qteSettings;
            _playerRepository = playerRepository;
            _timeProvider = timeProvider;
        }
        
        public void Initialize()
        {
            NetworkServer.RegisterHandler<QteResolveMessage>(OnQteResolve);
        }
        
        public void Dispose()
        {
            NetworkServer.UnregisterHandler<QteResolveMessage>();
        }

        public void StartQteSession(int connectionId)
        {
            var zoneWidth = UnityEngine.Random.Range(_qteSettings.MinSuccessZoneNormalizedWidth, 
                _qteSettings.MaxSuccessZoneNormalizedWidth);

            var zoneStart = UnityEngine.Random.Range(0f, 1f - zoneWidth);
            
            var conn = NetworkServer.connections[connectionId];
            
            conn.Send(new StartQteMessage
            {
                ZoneStart = zoneStart,
                ZoneWidth = zoneWidth
            });

            var player = _playerRepository.PlayerEntities[connectionId];
            var session = new QteSession(player.Color, zoneStart, zoneWidth);
            session.Completed += 
            _activeQteSessions.Add(connectionId, );
        }
        
        private void OnQteResolve(
            NetworkConnectionToClient conn, 
            QteResolveMessage msg
        )
        {
            var qteParams = _activeQteSessions[conn.connectionId];

            if (IsSuccess(msg.Value, qteParams.ZoneStart, qteParams.ZoneWidth))
                Success(conn);
            else Fail(conn);
        }

        private bool IsSuccess(
            float selectedValue, 
            float start, 
            float width
        )
        {
            return (selectedValue >= start && selectedValue <= start + width);
        }

        private void Success(NetworkConnectionToClient conn)
        {
            NetworkServer.SendToAll(new QteFailMessage());
        }
        
        private void Fail(NetworkConnectionToClient conn)
        {
            
        }

        private void OnSessionCompleted(Ac)
        {
            
        }

        public void Tick()
        {
            foreach (var sessionEntry in _activeQteSessions)
            {
                sessionEntry.Value.Update();
            }
        }
    }
}