using System.Collections.Generic;
using Entitites;
using Factories;
using Mirror;
using Services.Map;
using Services.Spawn;
using Settings;
using UnityEngine;

namespace Services.FlagSpawn.Impl
{
    public class FlagSpawnService : IFlagSpawnService
    {
        private readonly FlagEntityFactory _flagEntityFactory;
        private readonly Dictionary<EColor, List<FlagEntity>> _flagsEntities = new();
        private readonly FlagSettings _flagSettings;
        private readonly LevelSettings _levelSettings;
        private readonly ISpawnService _spawnService;

        public FlagSpawnService(
            ISpawnService spawnService, 
            LevelSettings levelSettings, 
            FlagEntityFactory gameEntityFactory,
            FlagSettings flagSettings
        )
        {
            _spawnService = spawnService;
            _levelSettings = levelSettings;
            _flagEntityFactory = gameEntityFactory;
            _flagSettings = flagSettings;
        }

        public IReadOnlyDictionary<EColor, List<FlagEntity>> Flags => _flagsEntities;

        public FlagEntity SpawnFlag(EColor color, int owner)
        {
            var position = Vector3.zero;
            var floorMax = _levelSettings.Floor.GetComponent<MeshFilter>().mesh.bounds.max * 3f;
            var floorMin = _levelSettings.Floor.GetComponent<MeshFilter>().mesh.bounds.min * 3f;

            position.x = Random.Range(floorMin.x, floorMax.x);
            position.z = Random.Range(floorMin.z, floorMax.z);

            var view = _spawnService.Spawn("Flag", position, Quaternion.identity);
            var flagEntity = _flagEntityFactory.Create();
            
            flagEntity.SetColor(color);
            flagEntity.ChangeCaptureTimeLeft(_flagSettings.CaptureTime);
            flagEntity.ChangeCaptureRadius(_flagSettings.CaptureRadius);
            flagEntity.SetPosition(view.Transform.position);
            flagEntity.IsServerObject = true;
            
            view.Initialize(flagEntity);
            var conn = NetworkServer.connections[owner];
            NetworkServer.Spawn(view.Transform.gameObject, conn);
            return flagEntity;
        }
    }
}