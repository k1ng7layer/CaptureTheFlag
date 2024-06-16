using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuView : UiView
    {
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _joinButton;
        [SerializeField] private TextMeshProUGUI _joinInfo;

        private Action _onHost;
        private Action _onJoin;

        public void Initialize(Action onHost, Action onJoin)
        {
            _onHost = onHost;
            _onJoin = onJoin;
            
            _hostButton.onClick.AddListener(OnHost);
            _joinButton.onClick.AddListener(OnJoin);
        }

        private void OnHost()
        {
            _hostButton.interactable = false;
            _onHost?.Invoke();
        }

        private void OnJoin()
        {
            _joinButton.interactable = false;
            _onJoin?.Invoke();
        }

        private void OnDestroy()
        {
            _hostButton.onClick.RemoveListener(OnHost);
            _hostButton.onClick.RemoveListener(OnJoin);
        }
    }
}