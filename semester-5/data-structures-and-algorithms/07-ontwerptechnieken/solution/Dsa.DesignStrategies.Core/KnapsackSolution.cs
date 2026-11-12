using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsa.DesignStrategies.Core
{
    public class KnapsackSolution
    {
        private List<KnapsackItem> Items { get; init; }

        public int Weight { get; private set; }
        public int Value { get; private set; }

        public KnapsackSolution()
        {
            Items = new List<KnapsackItem>();
        }

        public void Add(KnapsackItem item)
        {
            Items.Add(item);
            Weight += item.Weight;
            Value += item.Value;
        }

        public void RemoveLast()
        {
            int last = Items.Count - 1;

            KnapsackItem item = Items[last];
            Items.RemoveAt(last);
            
            Weight -= item.Weight;
            Value -= item.Value;
        }

        public KnapsackSolution Copy()
        {
            return new KnapsackSolution
            {
                Items = [.. Items],
                Weight = Weight,
                Value = Value
            };
        }

        public override string ToString()
        {
            return string.Join(",", Items);
        }

    }
}
