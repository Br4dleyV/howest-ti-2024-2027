public interface IHashTable<T> where T : notnull
{
    void Put(T item);
    bool Contains(T item);

    int Size { get; }
}