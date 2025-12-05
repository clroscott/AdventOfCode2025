using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Infrastructure.Tools.ProductID
{
    public interface IProductFinder
    {
        public bool IsValidProductID(string productID, int digitRepeat);

        public Tuple<long, long> FreshCounter(List<string> ranges, List<string> valuesToCheck);
    }
}
