namespace Entitites
{
    public interface IPropertyListener<T>
    {
        void OnPropertyChanged(T value);
    }
}