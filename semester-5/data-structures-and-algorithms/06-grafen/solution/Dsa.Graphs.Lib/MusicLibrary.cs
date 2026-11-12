
namespace Dsa.Graphs.Lib
{
    public class MusicLibrary
    {
        // map is used to be able to quickly update number of times listened to a song
        private Dictionary<string, int> counts = new();
        // priority queue for being able to quickly retrieve the top 10
        private PriorityQueue<string, int> queue = new();

        public void Add(string song)
        {
            counts.Add(song, 0);
            queue.Enqueue(song, 0);
        }

        public void ListenTo(string song)
        {
            if (!counts.ContainsKey(song))
            {
                Add(song);
            }

            counts[song]++;

            // negate count to get max- instead of min-heap
            queue.Enqueue(song, -counts[song]);
        }

        public List<string> GetTop10()
        {
            List<string> top10 = new();

            // get the top 10
            while (top10.Count < 10 && queue.TryDequeue(out string? song, out int count))
            {
                if (counts[song] == -count)
                {
                    top10.Add(song);
                }
            }

            // reinsert top 10 songs so queue stays intact
            foreach (string song in top10)
            {
                queue.Enqueue(song, -counts[song]);
            }

            return top10;
        }
    }
}
