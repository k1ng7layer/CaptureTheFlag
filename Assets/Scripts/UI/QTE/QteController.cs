using System;
using Services.Player;
using Services.QTE;
using Services.QTE.Client;
using Settings;
using UI.QteResult;
using UnityEngine;
using Zenject;

namespace UI.QTE
{
    public class QteController : UiController<QteView>, 
        IInitializable, 
        ITickable, 
        IDisposable
    {
        private readonly IQteClientService _qteClientService;
        private readonly PlayerRepository _playerRepository;
        private bool _opened;

        public QteController(
            IQteClientService qteClientService, 
            PlayerRepository playerRepository
        )
        {
            _qteClientService = qteClientService;
            _playerRepository = playerRepository;
        }
        
        public void Initialize()
        {
            _qteClientService.Started += BeginQteClient;
            _qteClientService.QteFailed += OnQteFail;
            View.gameObject.SetActive(false);
            View.Clicked += ResolveQte;
        }
        
        public void Dispose()
        {
            _qteClientService.Started -= BeginQteClient;
            View.Clicked -= ResolveQte;
        }

        private void OnQteFail(EColor loser)
        {
            if (!_opened)
                return;

            var loserIsLocal = loser == _playerRepository.LocalPlayer.Color;
            
            if (!loserIsLocal)
                return;
            
            Hide();
        }

        private void BeginQteClient(QteParams qteParams)
        {
            Show();
            View.InitializeSuccessZone(qteParams.ZoneStart, qteParams.ZoneWidth);
        }

        private void ResolveQte()
        {
            _qteClientService.Resolve();
            
            Hide();
        }

        private void Show()
        {
            View.Show();
            _opened = true;
        }

        private void Hide()
        {
            View.Hide();
            _opened = false;
        }

        public void Tick()
        {
            View.SetSliderValue(_qteClientService.CurrentValue);
        }
    }
}