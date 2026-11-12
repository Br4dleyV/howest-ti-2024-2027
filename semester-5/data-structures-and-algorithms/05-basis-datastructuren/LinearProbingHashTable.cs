using System;
using System.Text;

public class LinearProbingHashTable<T> : IHashTable<T> where T : notnull
{
    private T?[] _buckets;
    private readonly double _maxLoadFactor;
    public int Size { get; private set; }

    public LinearProbingHashTable(int initialCapacity = 4, double maxLoadFactor = 0.75)
    {
        _maxLoadFactor = maxLoadFactor;
        _buckets = new T?[initialCapacity];
    }

    public void Put(T item)
    {
        if (Size >= _buckets.Length * _maxLoadFactor)
            Resize();

        int index = GetIndex(item, _buckets.Length);
        while (_buckets[index] != null)
        {
            if (_buckets[index]!.Equals(item))
                return;
            index = (index + 1) % _buckets.Length;
        }
        _buckets[index] = item;
        Size++;
    }

    public bool Contains(T item)
    {
        int index = GetIndex(item, _values.Length);
        while (_buckets[index] != null)
        {
            if (_buckets[index]!.Equals(item))
                return true;
            index = (index + 1) % _buckets.Length;
        }
        return false;
    }

    private int Hash(T item)
    {
        int h = item.GetHashCode() % _buckets.Length;
        return h < 0 ? h + _buckets.Length : h;
    }

    private void Resize()
    {
        var old = _buckets;
        _buckets = new T?[_buckets.Length * 2];
        Size = 0;
        foreach (var item in old)
            if (item != null) Put(item);
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        for (int i = 0; i < _buckets.Length; i++)
            sb.AppendLine($"bucket: {i}:\n{_buckets[i]}");
        return sb.ToString();
    }
}