using Dsa.DesignStrategies.Core;

void Exercise1()
{
    Console.WriteLine("\nExercise 1:\n");

    NumberPuzzleSolver solver = new();

    List<int?[]> puzzles = [
        [null, null, 3, null, null, 1, 6],
        [null, null, null, null, null, 1, 6],
        [1, null, 3, null, 5, null, 7],
    ];

    foreach (int?[] puzzle in puzzles)
    {
        Console.WriteLine($"\nPuzzle:\n{string.Join(",", puzzle)}");
        Console.WriteLine("Solutions:");
        foreach (int[] solution in solver.Solve(puzzle))
        {
            Console.WriteLine(string.Join(",", solution));
        }
    }
}

void Exercise2()
{
    Console.WriteLine("\nExercise 2:\n");

    KnapsackSolver solver = new();

    List<KnapsackItem> items = [
        new KnapsackItem { Name = "A", Value = 6, Weight = 2},
        new KnapsackItem { Name = "B", Value = 2, Weight = 7},
        new KnapsackItem { Name = "C", Value = 3, Weight = 4},
    ];

    int capacity = 6;

    KnapsackSolution solution = solver.Solve(items, capacity);

    Console.WriteLine($"Capacity: {capacity}");
    Console.WriteLine("Items:");
    Console.WriteLine(string.Join(",", items));
    Console.WriteLine("Optimal selection:");
    Console.WriteLine(solution);
}

void Exercise3()
{
    Console.WriteLine("\nExercise 3:\n");

    CoinChangeSolver solver = new();

    List<int[]> coinSystems = [
        [1, 2, 5, 10, 20 , 50, 100, 200],   // euro coins (canonical)
        [1, 2, 5, 10, 20 , 25, 50]          // non-canonical system
    ];

    List<int> amounts = [8, 23, 35, 40, 150, 999];

    foreach (int[] coinSystem in coinSystems)
    {
        foreach (int amount in amounts)
        {
            int minCoins = solver.Solve(amount, coinSystem);
            Console.WriteLine($"\nMinimum coins to get {amount} " +
                $"with coins [{string.Join(",", coinSystem)}] = {minCoins}");
        }
    }
}

// run examples for selected exercises
Exercise1();
Exercise2();
Exercise3();