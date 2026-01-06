using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdventOfCode.Infrastructure.Tools.ProductID
{
    public class ProductFinder : IProductFinder
    {
        public bool IsValidProductID(string productID, int digitRepeat)
        {
            bool isValid = true;

            int digitLength = productID.ToString().Length;

            if (digitLength % digitRepeat != 0)
            {
                isValid = true;//this means it must be valid. no possible repeats
            }
            else
            {

                string previousID = "";
                for (int i = 0; i < digitRepeat; ++i)
                {
                    int startIndex = i * (digitLength / digitRepeat);

                    string currentID = productID.Substring(startIndex, (digitLength / digitRepeat));
                    if (previousID == "")
                    {
                        previousID = currentID;
                    }
                    else if (previousID != currentID)
                    {
                        isValid = true;
                        break;
                    }

                    isValid = false;//if it reaches here, both at the same, continue checking
                }

            }
            return isValid;
        }

        public Tuple<long, long> FreshCounter(List<string> ranges, List<string> valuesToCheck)
        {



            long freshFruitListed = 0;
            long totalPotentialFresh = 0;

            #region Consolidate Ranges

            List<(long start, long end)> rangeInt = new List<(long start, long end)>();

            rangeInt.AddRange(ranges.ConvertAll(range =>
            {
                var parts = range.Split('-');
                return (start: long.Parse(parts[0].Trim()), end: long.Parse(parts[1].Trim()));
            }));

            rangeInt = rangeInt.OrderBy(x => x.start).ToList();
            List<(long start, long end)> consolidatedRanges = new List<(long start, long end)>();

            foreach (var item in rangeInt)
            {
                if (consolidatedRanges.Count == 0)
                {
                    consolidatedRanges.Add(item);
                }
                else
                {
                    var previousRange = consolidatedRanges[consolidatedRanges.Count - 1];
                    if (item.start >= previousRange.start && item.start <= previousRange.end)
                    {
                        var temp = consolidatedRanges[consolidatedRanges.Count - 1].end;

                        consolidatedRanges[consolidatedRanges.Count - 1] = (previousRange.start, Math.Max(previousRange.end, item.end));


                    }
                    else
                    {
                        consolidatedRanges.Add(item);
                    }
                }

            }

            #endregion Consolidate Ranges


            foreach (var item in consolidatedRanges)
            {
                var tempVal = (item.end - item.start + 1);
                Console.WriteLine(item + " " + tempVal);

                totalPotentialFresh += tempVal;
            }


            foreach (var val in valuesToCheck)
            {
                long value = long.Parse(val.Trim());

                foreach (var item in consolidatedRanges)
                {
                    if (item.start <= value && value <= item.end)
                    {
                        Console.WriteLine($"Value {value} is in range {item.start}-{item.end}");
                        freshFruitListed++;
                        break;
                    }
                }
            }

            //Console.WriteLine($"Total Potential Fresh: {totalPotentialFresh}");



            Tuple<long, long> result = new Tuple<long, long>(freshFruitListed, totalPotentialFresh);

            return result;
        }
    }
}
