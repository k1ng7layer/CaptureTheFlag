namespace Entitites
{
    public interface IPropertyObservable<T>
    {
        void AddPropertyListener(IPropertyListener<T> listener);
    }
}