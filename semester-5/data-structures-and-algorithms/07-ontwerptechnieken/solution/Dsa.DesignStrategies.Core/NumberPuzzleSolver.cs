using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsa.DesignStrategies.Core
{
    public class NumberPuzzleSolver
    {
        // input array contains some numbers, empty positions are null
        public List<int[]> Solve(int?[] puzzle)
        {
            List<int[]> solutions = new();
            HashSet<int> numbersLeft = new();

            for (int number = 1; number <= puzzle.Length; number++)
            {
                if (!puzzle.Contains(number))
                    numbersLeft.Add(number); 
            }

            GenerateSolutions(puzzle, 0, numbersLeft, solutions);

            return solutions;
        }

        private void GenerateSolutions(int?[] subSolution, int currentIndex, HashSet<int> numbersLeft, List<int[]> fullSolutions)
        {
            if (currentIndex >= subSolution.Length)
            {
                // we found a solution!
                fullSolutions.Add(subSolution.Cast<int>().ToArray());
                return;
            }
            
            // try only valid numbers for current position
            List<int> validNumbers = GetValidNumbers(subSolution, currentIndex, numbersLeft);
            foreach (int number in validNumbers)
            {
                bool prefilled = subSolution[currentIndex] != null;

                // add chosen number
                subSolution[currentIndex] = number;
                numbersLeft.Remove(number);

                // recursively extend sub-solution
                GenerateSolutions(subSolution, currentIndex + 1, numbersLeft, fullSolutions);

                // remove chosen number to get ready for the next option to be tried
                // !! only if position wasn't prefilled (in that case it should be left as is)
                if (!prefilled)
                {
                    subSolution[currentIndex] = null;
                    numbersLeft.Add(number);
                }
            }
        }

        private List<int> GetValidNumbers(int?[] subSolution, int currentIndex, HashSet<int> numbersLeft)
        {
            if (subSolution[currentIndex] != null)
            {
                // position is pre-filled in the puzzle so we cannot change it
                return [subSolution[currentIndex]!.Value];
            }
            
            // only try numbers that haven't been used before (numbersLeft)
            // and differ by at least two from numbers filled in at adjacent positions
            return numbersLeft.Where(number =>
                DiffersByAtLeastTwo(subSolution, currentIndex - 1, number)
                && DiffersByAtLeastTwo(subSolution, currentIndex + 1, number)
            ).ToList();
        }

        private int? TryGetValue(int?[] subSolution, int index)
        {
            if (index < 0 || index >= subSolution.Length)
                return null;
            return subSolution[index];
        }

        private bool DiffersByAtLeastTwo(int?[] subSolution, int index, int newNumber)
        {
            int? numberInPuzzle = TryGetValue(subSolution, index);

            if (numberInPuzzle == null)
                return true;

            return Math.Abs(numberInPuzzle.Value - newNumber) >= 2;
        }
    }
}
