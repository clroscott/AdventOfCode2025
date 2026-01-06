using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Infrastructure.Tools.Dimension
{
    public class EuclideanCoordinate
    {

        public EuclideanCoordinate()
        {
            coordinates = new List<long>();
        }

        public EuclideanCoordinate(string coordinateString)
        {
            coordinates = new List<long>();
            if (string.IsNullOrWhiteSpace(coordinateString)) return;
            string[] parts = coordinateString.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                if (long.TryParse(part, out long value))
                {
                    coordinates.Add(value);
                }
            }
        }
        public EuclideanCoordinate(List<long> coords)
        {
            coordinates = coords ?? new List<long>();
        }
        
        public List<long> coordinates { get; set; } = new List<long>();
        public int coordinateDimension
        {
            get
            {
                if (coordinates == null) return 0;

                return coordinates.Count;
            }
        }

        public override string ToString()
        {
            return "(" + string.Join(", ", coordinates) + ")";
        }
    }
}
