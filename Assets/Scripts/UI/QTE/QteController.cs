using System;
using Services.QTE;
using Services.QTE.Client;
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

        public QteController(IQteClientService qteClientService)
        {
            _qteClientService = qteClientService;
        }
        
        public void Initialize()
        {
            _qteClientService.Started += BeginQteClient;
            View.Clicked += ResolveQte;
        }
        
        public void Dispose()
        {
            _qteClientService.Started -= BeginQteClient;
            View.Clicked -= ResolveQte;
        }

        private void BeginQteClient(QteParams qteParams)
        {
            View.InitializeSuccessZone(qteParams.ZoneStart, qteParams.ZoneWidth);
        }

        private void ResolveQte()
        {
            _qteClientService.Resolve();
        }

        public void Tick()
        {
            View.SetSliderValue(_qteClientService.CurrentValue);
        }
    }
}