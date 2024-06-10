using System;
using Mirror;
using Services.Input.Impl;
using Services.MessageDispatcher;
using Services.Network;
using Services.Player;
using Services.Spawn;
using Settings;
using Systems;
using UI.Input;
using UnityEngine;

namespace Core
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private JoystickView _joystickView;
        [SerializeField] private NetManager _networkManager;
        [SerializeField] private PrefabBase _prefabBase;

        private PlayerHandler _playerHandler;
        
        private void Start()
        {
            var inputService = new InputService();
            var inputController = new InputController(inputService);
            var dispatcherService = new MirrorMessageDispatcher();
            var playerSpawnService = new NetworkSpawnService(dispatcherService, _prefabBase);
            _playerHandler = new PlayerHandler(playerSpawnService, inputService);
            
            _playerHandler.Init();
            playerSpawnService.Init();
            inputController.Init();
        }

        private void Update()
        {
            if (_playerHandler != null)
                _playerHandler.Tick();
        }
    }
}