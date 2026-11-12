using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsa.DesignStrategies.Core
{
    public class CoinChangeSolver
    {
        public int Solve(int amount, int[] coins)
        {
            int[] table = new int[amount + 1];

            for (int i = 1; i <= amount; i++)
            {
                // try all coins that are not larger then current amount i
                int min = int.MaxValue;
                foreach (int c in coins.Where(c => c <= i))
                {
                    min = Math.Min(min, 1 + table[i - c]);
                }
                table[i] = min;
            }

            return table[amount];
        }
    }
}
