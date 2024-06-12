namespace Services.Time
{
    public interface ITimeProvider
    {
        float DeltaTime { get; }
    }
}