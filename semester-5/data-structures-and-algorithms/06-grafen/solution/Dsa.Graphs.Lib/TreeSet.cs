namespace Dsa.Graphs.Lib
{
    public class TreeSet
    {
        public BinaryTreeVertex Root { get; }
        public int Size { get; private set; }

        public TreeSet(int value)
        {
            Root = new(value);
            Size = 1;
        }

        public void Add(int value)
        {
            BinaryTreeVertex? currentVertex = Root;
            BinaryTreeVertex currentVertexParent = Root; // dummy value
            BinaryTreeVertex newVertex = new(value);

            while (currentVertex != null && currentVertex.Value != value)
            {
                currentVertexParent = currentVertex;
                if (currentVertex.Value > value)
                    currentVertex = currentVertex.Left;
                else
                    currentVertex = currentVertex.Right;
            }

            if (currentVertex != null) throw new ArgumentException("Key already exists");

            if (currentVertexParent.Value > value)
                currentVertexParent.Left = newVertex;
            else
                currentVertexParent.Right = newVertex;

            newVertex.Parent = currentVertexParent;
            Size++;
        }

        public int CountInRange(int start, int end)
        {
            return CountInRange(Root, start, end);
        }

        private int CountInRange(BinaryTreeVertex? vertex, int start, int end)
        {
            if (vertex == null)
                return 0;

            // count current node if its value falls in given range
            int count = vertex.Value >= start && vertex.Value <= end ? 1 : 0;

            if (vertex.Value >= start)
            {
                // we might still find smaller values also in the range
                count += CountInRange(vertex.Left, start, end);
            }
            if (vertex.Value <= end)
            {
                // we might still find larger values also in the range
                count += CountInRange(vertex.Right, start, end);
            }

            return count;
        }
    }
}
