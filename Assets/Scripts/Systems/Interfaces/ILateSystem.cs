namespace Systems
{
    public interface ILateSystem : ISystem
    {
        void OnLate();
    }
}