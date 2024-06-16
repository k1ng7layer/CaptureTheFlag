using System;
using Settings;

namespace Services.QTE.Client
{
    public interface IQteClientService
    {
        float CurrentValue { get; }
        bool Running { get; }
        event Action<EColor> QteCompleted;
        event Action<EColor> QteFailed;
        event Action<QteParams> Started;
        void Resolve();
    }
}