using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsa.DesignStrategies.Core
{
    public class KnapsackItem
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
