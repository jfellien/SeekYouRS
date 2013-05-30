namespace SeekYouRS.Storing
{
    public interface IStoreModels
    {
        T Retrieve<T>(dynamic query);
        void Add(dynamic model);
        void Change(dynamic model);
        void Remove(dynamic model);
    }
}