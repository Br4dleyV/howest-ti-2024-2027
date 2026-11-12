using Dsa.Graphs.Lib;

void Exercise3()
{
    Console.WriteLine("\nExercise 3:\n");

    // list values to be added to the set
    int[] values = [46, 74, 38, 61, 41, 47, 15, 82, 18, 11, 64, 79, 100, 76, 60, 65, 45, 30, 97, 92, 68, 25, 81, 80, 17, 54, 22, 27, 89, 12, 7, 73, 96, 40];
    
    // create binary search tree
    TreeSet set = new TreeSet(values[0]);
    // add all values
    for (int i = 1; i < values.Length; i++)
    {
        set.Add(values[i]);
    }

    // query some ranges
    int[][] ranges = [
            [38, 40],
            [1,  100],
            [40, 60],
            [1,  5]
        ];

    foreach (int[] range in ranges)
    {
        int count = set.CountInRange(range[0], range[1]);
        Console.WriteLine(
            $"Set of size {set.Size} contains {count} values in range [{range[0]}, {range[1]}]."
        );
    }
}

void Exercise4()
{
    Console.WriteLine("\nExercise 4:\n");

    MusicLibrary library = new MusicLibrary();

    // add some invented song titles
    string[] songs = [
                "Midnight Echo", "Starlight Avenue", "Crimson Sky", "Whispering Winds",
                "Neon Dreams", "Golden Horizon", "Velvet Rain", "Frozen Tide",
                "Echoes of Tomorrow", "Silent Reverie", "Twilight Serenade", "Burning Embers"
            ];

    // simulate listening randomly
    var random = new Random();
    for (int i = 0; i < 150; i++)
    {
        string songToPlay = songs[random.Next(songs.Length)];
        library.ListenTo(songToPlay);
    }

    // retrieve top 10 songs
    List<string> top10 = library.GetTop10();

    Console.WriteLine("Top 10 Songs (most listened):");
    for (int i = 0; i < top10.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {top10[i]}");
    }
}

void Exercise5()
{
    Console.WriteLine("\nExercise 5:\n");

    // Example 1: connected undirected graph (4 nodes)
    bool[][] connectedGraph =
    [
        [false, true,  true,  false],
        [true,  false, true,  true],
        [true,  true,  false, false],
        [false, true,  false, false]
    ];

    Console.WriteLine("Connected Graph is connected: " + AdjacencyMatrix.IsConnected(connectedGraph));

    // Example 2: disconnected undirected graph (4 nodes)
    bool[][] disconnectedGraph =
    [
        [false, true,  false, false],
        [true,  false, false, false],
        [false, false, false, true],
        [false, false, true,  false]
    ];

    Console.WriteLine("Disconnected Graph is connected: " + AdjacencyMatrix.IsConnected(disconnectedGraph));

    // Example 3: single node graph
    bool[][] singleNodeGraph =
    [
        [false]
    ];

    Console.WriteLine("Single Node Graph is connected: " + AdjacencyMatrix.IsConnected(singleNodeGraph));

    // Example 4: fully connected 3-node graph
    bool[][] fullGraph =
    [
        [false, true,  true],
        [true,  false, true],
        [true,  true,  false]
    ];

    Console.WriteLine("Fully Connected Graph is connected: " + AdjacencyMatrix.IsConnected(fullGraph));
}

void Exercise6()
{
    Console.WriteLine("\nExercise 6:\n");

    MovieLibrary movieLib = new MovieLibrary();

    List<string> actors = [
        "Kevin Bacon",
        "Michael Fassbender",
        "Ian McKellen",
        "James McAvoy"
    ];

    actors.AddRange(movieLib.GetRandomActors(10));
    
    foreach (string actor in actors)
    {
        int baconNumber = movieLib.GetBaconNumber(actor);
        Console.WriteLine($"Bacon number of {actor} is {baconNumber}.");
    }
}

// run example for selecton of exercises
Exercise3();
Exercise4();
Exercise5();
Exercise6();