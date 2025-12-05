using AdventOfCode.Infrastructure.Common.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Infrastructure.Tools.PowerBank
{
    public class PowerBank : IPowerBank
    {
        public long JolatgeOutput(string bankInput, int VolatageCount)
        {
            long result = 0;
            var intBank = ConvertUtil.StringToIntList(bankInput);

            Dictionary<int, List<int>> indexes = Enumerable
                .Range(0, 10).OrderByDescending(i => i)
                .ToDictionary(i => i, i => new List<int>());


            for (int i = 0; i < intBank.Count; ++i)
            {
                indexes[intBank[i]].Add(i);
            }

            //foreach(var item in indexes)
            //{
            //    Console.WriteLine($"Key: {item.Key}, Values: {string.Join(", ", item.Value)}");
            //}

            List<int> finalIndexes = new List<int>();

            for (int i = 0; i < VolatageCount; ++i)
            {
                int PreviousIndex = 0;

                if (i > 0)
                {
                    PreviousIndex = finalIndexes[i - 1];
                }
                var maxIndex = intBank.Count - (VolatageCount-i);



                var nextIndex = indexes.Where(x => x.Value.Count > 0 &&
                                              x.Value.Where(y => (i == 0 || y > PreviousIndex) && y <= maxIndex).ToList().Count > 0
                                        ).ToList()
                                        .OrderByDescending(x => x.Key)
                                        .First().Value
                                        .Where(y => (i == 0 || y > PreviousIndex) && y <= maxIndex)
                                        .Min();

                finalIndexes.Add(nextIndex);
            }

            #region pt 1 answer when just 2
            //var FirstIndex = indexes
            //                    .Where(i => i.Value.Count > 0 && !(i.Value.Count == 1 && i.Value.Max() == (intBank.Count - 1)))
            //                    .OrderByDescending(i => i.Key)
            //                    .First().Value
            //                    .Where(i => i < intBank.Count - 1)
            //                    .Min();

            //var secondIndex = indexes
            //                    .Where(i => i.Value.Count > 0 && i.Value.Max() > FirstIndex)
            //                    .OrderByDescending(i => i.Key)
            //                    .First().Value
            //                    .Min();

            #endregion pt 1 answer when just 2

            List<int> finalResult = finalIndexes.Select(i=> intBank[i]).ToList();

            result = Convert.ToInt64(string.Join("", finalResult));

            return result;
        }
    }
}
