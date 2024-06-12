using System.Collections.Generic;
using Entitites;
using Factories;
using Mirror;
using Services.Map;
using Services.Spawn;
using Settings;
using UnityEngine;
using Views;
using Zenject;

namespace Services.Flags.Impl
{
    public class FlagsService : IFlagsService
    {
        private readonly ISpawnService _spawnService;
        private readonly MapSettings _mapSettings;
        private readonly FlagEntityFactory _flagEntityFactory;
        private readonly FlagPresenterFactory _flagPresenterFactory;
        private readonly FlagSettings _flagSettings;
        private readonly Dictionary<EColor, FlagEntity> _flagsEntities = new();

        public FlagsService(
            ISpawnService spawnService, 
            MapSettings mapSettings, 
            FlagEntityFactory gameEntityFactory,
            FlagPresenterFactory flagPresenterFactory,
            FlagSettings flagSettings
        )
        {
            _spawnService = spawnService;
            _mapSettings = mapSettings;
            _flagEntityFactory = gameEntityFactory;
            _flagPresenterFactory = flagPresenterFactory;
            _flagSettings = flagSettings;
        }

        public IReadOnlyDictionary<EColor, FlagEntity> Flags => _flagsEntities;

        public void SpawnFlag(EColor color)
        {
            var position = Vector3.zero;
            var floorMax = _mapSettings.Floor.GetComponent<MeshFilter>().mesh.bounds.max * 3f;
            var floorMin = _mapSettings.Floor.GetComponent<MeshFilter>().mesh.bounds.min * 3f;

            position.x = Random.Range(floorMin.x, floorMax.x);
            position.z = Random.Range(floorMin.z, floorMax.z);

            var view = _spawnService.Spawn<IFlagView>("Flag", position, Quaternion.identity);
            
            NetworkServer.Spawn(view.Transform.gameObject);

            var flagEntity = _flagEntityFactory.Create();
            flagEntity.SetColor(color);
            flagEntity.ChangeCaptureTimeLeft(_flagSettings.CaptureTime);
            flagEntity.ChangeCaptureRadius(_flagSettings.CaptureRadius);
            flagEntity.SetPosition(position);
            
            var flagPresenter = _flagPresenterFactory.Create(view, flagEntity);
            flagPresenter.Initialize();
            
            _flagsEntities.Add(color, flagEntity);
        }
    }
}