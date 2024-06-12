namespace Services.Time.Impl
{
    public class UnityTimeProvider : ITimeProvider
    {
        public float DeltaTime => UnityEngine.Time.deltaTime;
    }
}