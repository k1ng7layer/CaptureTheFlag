using System;

namespace Services.QTE.Client
{
    public interface IQteClientService
    {
        event Action<QteParams> Started;
        float CurrentValue { get; }
        void Resolve();
    }
}