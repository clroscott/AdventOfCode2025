using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Infrastructure.Common.Converters
{
    public static class ConvertUtil
    {
        public static List<int> StringToIntList(string strValue)
        {
            List<int> intList = new List<int>();
            foreach (var str in strValue)
            {
                if (int.TryParse(str.ToString(), out int value))
                {
                    intList.Add(value);
                }
                else
                {
                    throw new FormatException($"Unable to convert '{str}' to an integer.");
                }
            }
            return intList;
        }
    }
}
