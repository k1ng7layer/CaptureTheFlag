namespace Views
{
    public interface IFlagView : IEntityView
    {
        void SetCaptureRadius(float value);
        void SetCaptured(bool value);
    }
}