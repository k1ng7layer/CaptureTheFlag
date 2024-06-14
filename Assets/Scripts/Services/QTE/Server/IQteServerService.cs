using Settings;

namespace Services.QTE.Server
{
    public interface IQteServerService
    {
        void StartQteSession(int connectionId);
    }
}