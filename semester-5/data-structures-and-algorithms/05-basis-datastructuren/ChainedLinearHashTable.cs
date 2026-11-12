public class ChainedLinearHashTable<T> : IHashTable<T> where T : notnull
{
    
    private int _nrOfBucketsInUse;
    private int _hashMod;
    public int Size { get; private set; }
    private readonly double _maxLoadFactor;
    private LinkedList<T>[] _buckets;

    public ChainedLinearHashTable(int nrOfBuckets = 4, double maxLoadFactor = 0.75)
    {
        _nrOfBucketsInUse = nrOfBuckets;
        _hashMod = CalculateNextPowerOfTwo(nrOfBuckets);
        Size = 0;
        _maxLoadFactor = maxLoadFactor;
        _buckets = new LinkedList<T>[_nrOfBucketsInUse];
        for (int i = 0; i < _nrOfBucketsInUse; i++) _buckets[i] = new LinkedList<T>();
    }

    public void Put(T item)
    {
        if (Contains(item)) return;

        double newLoad = (double) (1 + Size) / _nrOfBucketsInUse;
        if (newLoad > _maxLoadFactor) Resize();

        int hash = CalculateNeededHash(item);
        _buckets[hash].AddFirst(item);
        Size++;
    }
    
    public bool Contains(T item)
    {
        // Check if the item is null
        if (item == null) return false;

        // Calculate the hash for the item
        int bucketIndex = CalculateNeededHash(item);

        // Search the item in the LinkedList of the corresponding bucket
        return _buckets[bucketIndex].Contains(item);
    }
    
    private static int CalculateNextPowerOfTwo(int number)
    {
        int value = 1;
        while (value < number)
        {
            value <<= 1;
        }
        return value;
    }

    private int CalculateNeededHash(T item)
    {
        int fullHash = item.GetHashCode();
        int partOfHashToWatch = fullHash % _hashMod;
        if (partOfHashToWatch < 0) partOfHashToWatch += _hashMod;

        // The bucket for this hash value is not (yet) in use
        if (partOfHashToWatch >= _nrOfBucketsInUse) partOfHashToWatch = fullHash % (_hashMod / 2);
        
        return partOfHashToWatch;
    }
    
    private void Resize()
    {
        if (_nrOfBucketsInUse == _buckets.Length)
        {
            Array.Resize(ref _buckets, _buckets.Length * 2);
            for (int i = _nrOfBucketsInUse; i < _buckets.Length; i++) _buckets[i] = new LinkedList<T>();
            _hashMod *= 2;
        }

        _nrOfBucketsInUse++;
        
        int indexToRedistribute = (_nrOfBucketsInUse - 1) % (_hashMod / 2);
        LinkedList<T> elementsToRedistribute = _buckets[indexToRedistribute];
        _buckets[indexToRedistribute] = new LinkedList<T>();

        foreach (T item in elementsToRedistribute)
        {
            int hash = CalculateNeededHash(item);
            _buckets[hash].AddFirst(item);
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _nrOfBucketsInUse; i++)
        {
            sb.AppendLine($"bucket: {i}:");
            foreach (T item in _buckets[i])
            {
                sb.Append(item.ToString() + " ");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}