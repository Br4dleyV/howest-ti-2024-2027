using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Dsa.Graphs.Lib
{
    public class MovieLibrary
    {
        private static readonly Random random = new Random();
        private static readonly string dataPath 
            = Path.Combine(AppContext.BaseDirectory, "Data", "movies.csv");

        // adjacency list: key = actor, value = list of actors they worked with
        private readonly Dictionary<string, HashSet<string>> coActors = [];

        public MovieLibrary()
        {
            LoadData();
        }

        private void LoadData()
        {
            if (!File.Exists(dataPath))
                throw new FileNotFoundException("Movies file not found", dataPath);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };

            using var reader = new StreamReader(dataPath);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<Movie>();

            foreach (var movie in records)
            {
                var actorsInMovie = ParseActorList(movie.Stars);
                AddActorsToGraph(actorsInMovie);
            }
        }

        private List<string> ParseActorList(string starsField)
        {
            if (string.IsNullOrWhiteSpace(starsField))
                return [];

            return starsField
                .Trim('[', ']')                         
                .Split(['\''], StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim().Trim(','))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
        }

        private void AddActorsToGraph(List<string> actorsInMovie)
        {
            // register any actors that were not playing in any previous movie
            for (int i = 0; i < actorsInMovie.Count; i++)
            {
                string actor = actorsInMovie[i];
                if (!coActors.ContainsKey(actor))
                    coActors[actor] = [];
            }
            
            // update graph: connect all actors playing in this movie
            for (int i = 0; i < actorsInMovie.Count; i++)
            {
                string actor1 = actorsInMovie[i];
                for (int j = i + 1; j < actorsInMovie.Count; j++)
                {   
                    string actor2 = actorsInMovie[j];
                    coActors[actor1].Add(actor2);
                    coActors[actor2].Add(actor1);
                }
            }
        }

        public List<string> GetRandomActors(int count)
        {
            List<string> randomActors = new();
            for(int i = 0; i < count; i++)
            {
                int randomActorIndex = random.Next(coActors.Count);
                randomActors.Add(coActors.Keys.ElementAt(randomActorIndex));
            }

            return randomActors;
        }

        public int GetBaconNumber(string actor)
        {
            if (!coActors.ContainsKey(actor))
            {
                // unknown actor
                return -1;
            }

            // use BFS to find shortest path to Kevin Bacon
            int distance = 0;
            HashSet<string> visited = [ actor] ;
            Queue<string> queue = new();
            queue.Enqueue(actor);

            string bacon = "Kevin Bacon";

            while (queue.Count > 0 && !visited.Contains(bacon))
            {
                string nextActor = queue.Dequeue();

                foreach (string neighbour in coActors[nextActor])
                {
                    if (!visited.Contains(neighbour))
                    {
                        queue.Enqueue(neighbour);
                        visited.Add(neighbour);
                    }
                }

                distance++;
            }

            return distance;
        }
    }
}
