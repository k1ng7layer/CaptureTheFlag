﻿using System;
using System.Collections.Generic;
using Entitites;
using Services.Player;
using Services.QTE.Server;
using Services.Time;
using Settings;
using Zenject;
using Random = UnityEngine.Random;

namespace Systems.Server
{
    public class StartQteSystem : IInitializable, IUpdateSystem, IDisposable
    {
        private readonly IQteServerService _qteServerService;
        private readonly ITimeProvider _timeProvider;
        private readonly PlayerRepository _playerRepository;
        private readonly QteSettings _qteSettings;
        private readonly FlagSettings _flagSettings;
        private readonly Dictionary<int, QteDelayedStart> _pendingQte = new();
        private readonly List<QteDelayedStart> _inactive = new();

        public StartQteSystem(
            IQteServerService qteServerService,
            ITimeProvider timeProvider,
            PlayerRepository playerRepository,
            QteSettings qteSettings,
            FlagSettings flagSettings)
        {
            _qteServerService = qteServerService;
            _timeProvider = timeProvider;
            _playerRepository = playerRepository;
            _qteSettings = qteSettings;
            _flagSettings = flagSettings;
        }
        
        public void Initialize()
        {
            _playerRepository.Added += Init;
            _playerRepository.Removed += RemoveQte;
        }
        
        public void Dispose()
        {
            _playerRepository.Added -= Init;
            _playerRepository.Removed -= RemoveQte;
        }

        private void Init(PlayerEntity playerEntity)
        {
            playerEntity.CapturingChanged += PrepareQte;
        }
        
        private void PrepareQte(bool capturing, PlayerEntity player)
        {
            if (!capturing)
            {
                _pendingQte.Remove(player.OwnerId);
                return;
            }
            
            var chance = Random.Range(0f, 1f);
            var startTime = Random.Range(0f, _flagSettings.CaptureTime - _qteSettings.Duration);
            
            // if (chance >= 0.5f)
            //     return;

            var delayedStart = new QteDelayedStart(player.OwnerId, 1f);

            delayedStart.Elapsed += BeginQte;
            
            _pendingQte.Add(player.OwnerId, delayedStart);
        }

        private void BeginQte(QteDelayedStart qteDelayedStart)
        {
            qteDelayedStart.Elapsed -= BeginQte;

            _inactive.Add(qteDelayedStart);
            
            _qteServerService.StartQteSession(qteDelayedStart.ConnectionId);
        }

        private void RemoveQte(PlayerEntity entity)
        {
            entity.CapturingChanged -= PrepareQte;
            _pendingQte.Remove(entity.OwnerId);
        }

        public void Update()
        {
            foreach (var qteDelayedStart in _pendingQte)
            {
                qteDelayedStart.Value.Tick(_timeProvider.DeltaTime);
            }

            foreach (var inactive in _inactive)
            {
                _pendingQte.Remove(inactive.ConnectionId);
            }
            
            if (_inactive.Count > 0)
                _inactive.Clear();
        }
    }
}