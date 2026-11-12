using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsa.DesignStrategies.Core
{
    public class KnapsackSolver
    {
        private int[] remainingValues;
        private int[] minimumWeights;
        private int[] maximumValues;

        public KnapsackSolution Solve(List<KnapsackItem> items, int capacity)
        {
            remainingValues = new int[items.Count + 1];
            minimumWeights = new int[items.Count + 1];
            maximumValues = new int[items.Count + 1];

            // Precompute the total value, minimum weight and maximum value
            // of the remaining items for every starting index.
            minimumWeights[items.Count] = int.MaxValue;
            for (int i = items.Count - 1; i >= 0; i--)
            {
                remainingValues[i] = remainingValues[i + 1] + items[i].Value;
                minimumWeights[i] = Math.Min(items[i].Weight, minimumWeights[i + 1]);
                maximumValues[i] = Math.Max(items[i].Value, maximumValues[i + 1]);
            }

            return Solve(items, capacity, new(), new(), 0);
        }

        private KnapsackSolution Solve(List<KnapsackItem> items, int capacity,
            KnapsackSolution subSolution, KnapsackSolution bestSolution, int currentIndex)
        {
            if (subSolution.Weight > capacity) {
                // Backtracking: already overweight
                return bestSolution;
            } 

            if (currentIndex >= items.Count)
            {
                // done! made decision for all items
                if (subSolution.Value > bestSolution.Value)
                {
                    // found new best solution
                    // need to take a copy here to permanently store it
                    // Extra exercise: how does this influence the time complexity, and can we improve this further?
                    return subSolution.Copy();
                }

                return bestSolution;
            }

            // Bound 1: even adding the value of all remaining items cannot improve it.
            if (subSolution.Value + remainingValues[currentIndex] <= bestSolution.Value)
            {
                // Branch-and-bound: cannot lead to optimal solution
                return bestSolution;
            }

            // Bound 2: at most this many items fit, each worth at most the maximum value.
            double maxNewItems = (double)(capacity - subSolution.Weight) / minimumWeights[currentIndex];
            if (subSolution.Value + maxNewItems * maximumValues[currentIndex] <= bestSolution.Value)
            {
                // Branch-and-bound: cannot lead to optimal solution
                return bestSolution;
            }

            KnapsackItem item = items[currentIndex];

            // option 1: put item in knapsack
            subSolution.Add(item);
            bestSolution = Solve(items, capacity, subSolution, bestSolution, currentIndex + 1);

            // undo adding the item to be able to explore the other option
            subSolution.RemoveLast();

            // option 2: don't put next item in knapsack
            bestSolution = Solve(items, capacity, subSolution, bestSolution, currentIndex + 1);

            return bestSolution;
        }
    }
}
