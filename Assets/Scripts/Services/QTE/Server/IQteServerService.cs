using System;
using Services.QTE.Client;
using Settings;
using UI.QteResult;

namespace Services.QTE.Server
{
    public interface IQteServerService
    {
        event Action<ServerQteResultArgs> QteCompleted;
        void StartQteSession(int connectionId);
        void StopQteSession(int connectionId, EQteResult result = EQteResult.None);
    }
}