using System;
using Settings;

namespace Services.QTE.Client
{
    public interface IQteClientService
    {
        event Action<EColor> QteCompleted;
        event Action<EColor> QteFailed;
        event Action<QteParams> Started;
        float CurrentValue { get; }
        bool Running { get; }
        void Resolve();
    }
}