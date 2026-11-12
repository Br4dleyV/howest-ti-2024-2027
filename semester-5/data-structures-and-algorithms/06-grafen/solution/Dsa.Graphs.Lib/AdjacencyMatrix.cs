
namespace Dsa.Graphs.Lib
{
    public static class AdjacencyMatrix
    {
        public static bool IsConnected(bool[][] graph)
        {
            int n = graph.Length;
            bool[] visited = new bool[n];

            // traverse graph depth-first
            VisitNeighbours(graph, 0, visited);

            // check if all nodes have been visited
            return visited.All(b => b);
        }

        private static void VisitNeighbours(bool[][] graph, int node, bool[] visited)
        {
            // mark node itself as visited
            visited[node] = true;

            // visit neighbours depth-first
            for (int i = 0; i < graph.Length; i++)
            {
                // check if neighbour (edge from node -> i)
                // and skip if already visited
                if (graph[node][i] && !visited[i])
                {
                    VisitNeighbours(graph, i, visited);
                }
            }
        }
    }
}
