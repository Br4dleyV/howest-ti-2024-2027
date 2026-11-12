namespace Dsa.Graphs.Lib
{
    public class BinaryTreeVertex
    {
        public int Value { get; set; }
        public BinaryTreeVertex? Parent { get; set; } = null;
        public BinaryTreeVertex? Left { get; set; } = null;
        public BinaryTreeVertex? Right { get; set; } = null;

        public BinaryTreeVertex(int value)
        {
            Value = value;
        }
    }
}
