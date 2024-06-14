using System.Collections.Generic;
using Entitites;
using Factories;
using Mirror;
using Services.Map;
using Services.Spawn;
using Settings;
using UnityEngine;
using Views;

namespace Services.Flags.Impl
{
    public class FlagSpawnService : IFlagSpawnService
    {
        private readonly ISpawnService _spawnService;
        private readonly MapSettings _mapSettings;
        private readonly FlagEntityFactory _flagEntityFactory;
        private readonly FlagSettings _flagSettings;
        private readonly FlagRepository.Impl.FlagRepository _flagRepository;
        private readonly Dictionary<EColor, List<FlagEntity>> _flagsEntities = new();
        private readonly Dictionary<int, EColor> _pendingFlags = new();

        public FlagSpawnService(
            ISpawnService spawnService, 
            MapSettings mapSettings, 
            FlagEntityFactory gameEntityFactory,
            FlagSettings flagSettings,
            FlagRepository.Impl.FlagRepository flagRepository
        )
        {
            _spawnService = spawnService;
            _mapSettings = mapSettings;
            _flagEntityFactory = gameEntityFactory;
            _flagSettings = flagSettings;
            _flagRepository = flagRepository;
        }

        public IReadOnlyDictionary<EColor, List<FlagEntity>> Flags => _flagsEntities;

        public FlagEntity SpawnFlag(EColor color)
        {
            var position = Vector3.zero;
            var floorMax = _mapSettings.Floor.GetComponent<MeshFilter>().mesh.bounds.max * 3f;
            var floorMin = _mapSettings.Floor.GetComponent<MeshFilter>().mesh.bounds.min * 3f;

            position.x = Random.Range(floorMin.x, floorMax.x);
            position.z = Random.Range(floorMin.z, floorMax.z);

            var view = _spawnService.Spawn("Flag", position, Quaternion.identity);
           
            //view.ClientStarted += OnFlagReady;
            _pendingFlags.Add(view.Transform.GetHashCode(), color);

            //OnFlagReady(view);
            
            var flagEntity = _flagEntityFactory.Create();
            
            flagEntity.SetColor(color);
            flagEntity.ChangeCaptureTimeLeft(_flagSettings.CaptureTime);
            flagEntity.ChangeCaptureRadius(color == EColor.Blue ? 3 : _flagSettings.CaptureRadius);
            flagEntity.SetPosition(view.Transform.position);
            flagEntity.IsServerObject = true;
            
            view.Initialize(flagEntity);
            NetworkServer.Spawn(view.Transform.gameObject);
            return flagEntity;
        }

        private void OnFlagReady(IEntityView view)
        {
            view.ClientStarted -= OnFlagReady;
            
            var flagEntity = _flagEntityFactory.Create();
            var color = _pendingFlags[view.Transform.GetHashCode()];
            view.Initialize(flagEntity);
            
            flagEntity.SetColor(color);
            flagEntity.ChangeCaptureTimeLeft(_flagSettings.CaptureTime);
            flagEntity.ChangeCaptureRadius(_flagSettings.CaptureRadius);
            flagEntity.SetPosition(view.Transform.position);
            
            if (!_flagsEntities.ContainsKey(color))
                _flagsEntities.Add(color, new List<FlagEntity>());
            
            _flagsEntities[color].Add(flagEntity);

            _pendingFlags.Remove(view.GetHashCode());
            //_flagRepository.Add(flagEntity);
        }
    }
}