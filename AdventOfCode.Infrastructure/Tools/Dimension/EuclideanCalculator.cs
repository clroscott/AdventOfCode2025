using AdventOfCode.Infrastructure.Tools.Lock;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Infrastructure.Tools.Dimension
{
    public static class EuclideanCalculator
    {

        public static long ChainBuilding(List<string> fileContents)
        {
            long result = 0;

            List<(EuclideanCoordinate coordinate1, EuclideanCoordinate coordinate2, double distance)> coordinates = new List<(EuclideanCoordinate, EuclideanCoordinate, double)>();

            for (int i = 0; i < fileContents.Count; i++)
            {
                EuclideanCoordinate firstCoord = new EuclideanCoordinate(fileContents[i]);
                for (int j = i + 1; j < fileContents.Count; ++j)
                {
                    EuclideanCoordinate secondCoord = new EuclideanCoordinate(fileContents[j]);

                    coordinates.Add((firstCoord, secondCoord, CalculateDistance(firstCoord, secondCoord)));

                }
            }

            coordinates = coordinates.OrderBy(x => x.distance).ToList();

            List<List<EuclideanCoordinate>> chains = new List<List<EuclideanCoordinate>>();

            //coordinates = coordinates.Take(1).ToList();

            int connectioncount = 0;

            int dim1 = 0;
            int dim2 = 0;
            List<Tuple<long, long>> mergedChains = new List<Tuple<long, long>>();

            for (int i1 = 0; i1 < coordinates.Count; i1++)
            {
                //if (connectioncount >= 1000) break;

                (EuclideanCoordinate coordinate1, EuclideanCoordinate coordinate2, double distance) coordPair = coordinates[i1];
                Console.Write(coordPair.coordinate1.ToString() + " <-> " + coordPair.coordinate2.ToString() + " = " + coordPair.distance);

                if (chains.Count == 0)
                {
                    connectioncount++;
                    chains.Add(new List<EuclideanCoordinate> { coordPair.coordinate1, coordPair.coordinate2 });
                }
                else
                {
                    bool inAnyChain = false;
                    for (int i = 0; i < chains.Count; i++)
                    {
                        List<EuclideanCoordinate>? chain = chains[i];
                        var comparer = EqualityComparer<EuclideanCoordinate>.Create(
                            (a, b) =>
                            {
                                if (a.coordinateDimension != b.coordinateDimension)
                                {
                                    return false;
                                }
                                for (int dim = 0; dim < a.coordinateDimension; ++dim)
                                {
                                    if (a.coordinates[dim] != b.coordinates[dim])
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                            );
                        bool inChain1 = chain.Contains(coordPair.coordinate1, comparer);
                        bool inChain2 = chain.Contains(coordPair.coordinate2, comparer);


                        if (!inChain1 && !inChain2)
                        {
                            continue;
                        }
                        else if (inChain1 && inChain2)
                        {
                            connectioncount++;
                            inAnyChain = true;
                            break;
                        }
                        else
                        {
                            inAnyChain = true;

                            bool added = false;

                            for (int j = i; j < chains.Count; ++j)
                            {
                                if (inChain1)
                                {
                                    if (chains[j].Contains(coordPair.coordinate2, comparer))
                                    {
                                        //merge chains
                                        chains[i].AddRange(chains[j]);
                                        chains.RemoveAt(j);
                                        added = true;
                                        connectioncount++;
                                        break;
                                    }

                                }
                                else if (inChain2)
                                {
                                    if (chains[j].Contains(coordPair.coordinate1, comparer))
                                    {
                                        //merge chains
                                        chains[i].AddRange(chains[j]);
                                        chains.RemoveAt(j);
                                        added = true;
                                        connectioncount++;
                                        break;
                                    }
                                }
                            }
                            if (!added)
                            {
                                if (inChain1)
                                {
                                    chain.Add(coordPair.coordinate2);
                                    connectioncount++;
                                }
                                else
                                {
                                    chain.Add(coordPair.coordinate1);
                                    connectioncount++;
                                }
                                mergedChains.Add(new Tuple<long, long>((long)coordPair.coordinate1.coordinates[0], (long)coordPair.coordinate2.coordinates[0]));

                            }

                            break;
                        }

                    }

                    if (!inAnyChain)
                    {
                        chains.Add(new List<EuclideanCoordinate> { coordPair.coordinate1, coordPair.coordinate2 });
                        connectioncount++;
                    }
                }

                //if (coordPair.coordinate1.coordinates[0] == 216 || coordPair.coordinate1.coordinates[0] ==117)
                //{
                //    if (coordPair.coordinate2.coordinates[0] == 216 || coordPair.coordinate2.coordinates[0] == 117)
                //    {
                //        Console.Write($"!!!!!!!!!!!!!!!!!!!");
                //    }
                //}


                Console.WriteLine($" - Chains : {chains.Count}");

                //if (chains.Count == 1)
                //{
                //    //if (dim1 != 0)
                //    //{
                //    dim1 = (int)coordPair.coordinate1.coordinates[0];
                //    dim2 = (int)coordPair.coordinate2.coordinates[0];
                //    //}
                //}
                //else
                //{
                //    if (dim1 != 0 && dim2 != 0)
                //    {
                //        mergedChains.Add(new Tuple<int, int>(dim1, dim2));
                //    }

                //    dim1 = 0;
                //    dim2 = 0;
                //}

            }

            chains = chains.OrderByDescending(x => x.Count).ToList();

            //result = chains[0].Count * chains[1].Count * chains[2].Count;
            //result = dim1 * dim2;

            result =
            mergedChains.Last().Item1 *
            mergedChains.Last().Item2;
            //result = dim1 * dim2;



            return result;
        }



        public static char[][] grid;
        public static long RectangleFinder(List<string> fileContents)
        {
            long result = 0;


            long maxX = 0;
            long maxY = 0;
            long minX = long.MaxValue;
            long minY = long.MaxValue;





            List<(EuclideanCoordinate coord1, EuclideanCoordinate coord2, long area)> coordinates = new List<(EuclideanCoordinate coord1, EuclideanCoordinate coord2, long area)>();
            List<(long, long)> values = new List<(long, long)>();

            foreach (var line in fileContents)
            {
                var coord = line.Split(',');

                long x = Convert.ToInt64(coord[0]);
                long y = Convert.ToInt64(coord[1]);

                values.Add((x, y));

                if (x > maxX) maxX = x;
                if (y > maxY) maxY = y;
                if (x < minX) minX = x;
                if (y < minY) minY = y;

            }



            //long prevX = 0;
            //long prevY = 0;

            //long prevXinarow = 0;
            //long prevYinarow = 0;
            //var temp = values.Select(i=>i.Item2).Distinct().OrderBy(x => x).ToList();


            //for (int i = 0; i < temp.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        Console.WriteLine(temp[i]);
            //    }
            //    else
            //    {
            //        if (temp[i] - temp[i - 1] < 10)
            //        {
            //            Console.Write(temp[i]);
            //            Console.Write("-");
            //            Console.Write(temp[i-1]);
            //            Console.Write("-------dif------");
            //            Console.WriteLine(temp[i] - temp[i - 1]);
            //        }
            //    }
            //}

            //return 0;





            grid = new char[maxY + 2][];
            for (int i = 0; i < grid.Length; ++i)
            {
                grid[i] = new char[maxX + 2];
                for (int i1 = 0; i1 < grid[i].Length; ++i1)
                {
                    grid[i][i1] = ' ';
                }
            }



            //foreach (var row in grid)
            //{
            //    Console.WriteLine(new string(row));
            //}

            //Console.ReadLine();

            string dotdotdot = "...";
            EuclideanCoordinate previousCoord = null;
            EuclideanCoordinate originCoord = null;
            for (int i = 0; i < values.Count; i++)
            {
                EuclideanCoordinate firstCoord = new EuclideanCoordinate(new List<long> { values[i].Item1, values[i].Item2 });

                grid[firstCoord.coordinates[1]][firstCoord.coordinates[0]] = '#';

                if (previousCoord != null)
                {
                    if (previousCoord.coordinates[1] == firstCoord.coordinates[1])
                    {
                        long y = firstCoord.coordinates[1];
                        for (long x = Math.Min(previousCoord.coordinates[0], firstCoord.coordinates[0]); x <= Math.Max(previousCoord.coordinates[0], firstCoord.coordinates[0]); ++x)
                        {
                            if (grid[y][x] != '#')
                            {
                                grid[y][x] = 'X';
                            }
                        }
                    }
                    else if (previousCoord.coordinates[0] == firstCoord.coordinates[0])
                    {
                        long x = firstCoord.coordinates[0];
                        for (long y = Math.Min(previousCoord.coordinates[1], firstCoord.coordinates[1]); y <= Math.Max(previousCoord.coordinates[1], firstCoord.coordinates[1]); ++y)
                        {
                            if (grid[y][x] != '#')
                            {
                                grid[y][x] = 'X';
                            }
                        }
                    }
                }
                else
                {
                    originCoord = firstCoord;
                }
                previousCoord = firstCoord;


                for (int j = i + 1; j < values.Count; ++j)
                {
                    EuclideanCoordinate secondCoord = new EuclideanCoordinate(new List<long> { values[j].Item1, values[j].Item2 });
                    double area = CalculateArea(firstCoord, secondCoord);
                    coordinates.Add((firstCoord, secondCoord, (long)area));
                }

                Console.Write($"Outlining{dotdotdot}");
                if (i % 2 == 0)
                {
                    Console.WriteLine(dotdotdot);
                }
                else
                {
                    Console.WriteLine();
                }
            }

            if (originCoord != null && previousCoord != null)
            {
                if (originCoord.coordinates[1] == previousCoord.coordinates[1])
                {
                    long y = originCoord.coordinates[1];
                    for (long x = Math.Min(originCoord.coordinates[0], previousCoord.coordinates[0]); x <= Math.Max(originCoord.coordinates[0], previousCoord.coordinates[0]); ++x)
                    {
                        if (grid[y][x] != '#')
                        {
                            grid[y][x] = 'X';
                        }
                    }
                }
                else if (originCoord.coordinates[0] == previousCoord.coordinates[0])
                {
                    long x = originCoord.coordinates[0];
                    for (long y = Math.Min(originCoord.coordinates[1], previousCoord.coordinates[1]); y <= Math.Max(originCoord.coordinates[1], previousCoord.coordinates[1]); ++y)
                    {
                        if (grid[y][x] != '#')
                        {
                            grid[y][x] = 'X';
                        }
                    }
                }
            }

            //foreach (var row in grid)
            //{
            //    Console.WriteLine(new string(row));
            //}

            Console.WriteLine();
            Console.WriteLine("---Outline Generated---");
            Console.WriteLine();

            bool inside = false;
            char[] up2row = new char[0];
            char[] up1row = new char[0];

            for (int i = 0; i < grid.Length; ++i)
            {
                inside = false;
                up2row = i >= 2 ? grid[i - 2] : new char[0];
                up1row = i >= 1 ? grid[i - 1] : new char[0];

                for (int i1 = 0; i1 < grid[i].Length; ++i1)
                {
                    if (up2row.Length == 0)
                    {
                        //we are in one of the first 2 rows, that means any space is outside the shape
                        if (grid[i][i1] == ' ')
                        {
                            grid[i][i1] = '.';
                        }
                    }
                    else //we have at least 2 rows above us
                    {
                        if (grid[i][i1] == ' ')//either inside or outside
                        {
                            if (inside)
                            {
                                grid[i][i1] = 'X';

                            }
                            else
                            {
                                grid[i][i1] = '.';
                            }
                        }
                        else //we are on a border or corner ( 'X' or '#' )
                        {
                            if (grid[i][i1 - 1] == '.' || grid[i][i1] == '#' && up1row[i1 + 1] == 'X')
                            {
                                inside = true;
                            }
                            else
                            {
                                inside = false;
                            }
                        }
                    }


                }

                Console.Write($"Filling in{dotdotdot}");
                if (i % 2 == 0)
                {
                    Console.WriteLine(dotdotdot);
                }
                else
                {
                    Console.WriteLine();
                }
            }

            Console.WriteLine();

            //foreach (var row in grid)
            //{
            //    Console.WriteLine(new string(row));
            //}

            Console.WriteLine();
            Console.WriteLine("---Shape Filled In---");
            Console.WriteLine();




            coordinates = coordinates.OrderByDescending(x => x.area).ToList();

            //foreach (var coord in coordinates)
            //{
            //    Console.WriteLine($"{coord.coord1.ToString()} <-> {coord.coord2.ToString()} = Area: {coord.area}");
            //}

            var finalArea = coordinates.FirstOrDefault();
            bool foundAnswer = true;
            int index = 0;
            int mindex = coordinates.Count;
            foreach (var coord in coordinates)
            {
                Console.WriteLine($"{index++} / {mindex} ");

                long coordXStart = Math.Min(coord.coord1.coordinates[0], coord.coord2.coordinates[0]);
                long coordXEnd = Math.Max(coord.coord1.coordinates[0], coord.coord2.coordinates[0]);
                long coordYStart = Math.Min(coord.coord1.coordinates[1], coord.coord2.coordinates[1]);
                long coordYEnd = Math.Max(coord.coord1.coordinates[1], coord.coord2.coordinates[1]);

                foundAnswer = true;
                finalArea = finalArea = coord;

                //check outside in

                for (long y = coordYStart; y < coordYEnd+1; ++y)
                {
                    for (long x = coordXStart; x < coordXEnd; ++x)
                    {
                        if (grid[y][x] == '.' || grid[coordYEnd][x] == '.')//top and bottom rows
                        {
                            finalArea = coord;
                            foundAnswer = false;
                            break;
                        }
                        else
                        {
                            //doesnt matter
                        }
                    }
                    if (!foundAnswer)
                    {
                        break;
                    }

                    for(long y2 = coordYStart; y2 < coordYEnd; ++y2)
                    {
                        if (grid[y2][coordXStart] == '.' || grid[y2][coordXEnd] == '.')//left and right columns
                        {
                            finalArea = coord;
                            foundAnswer = false;
                            break;
                        }
                        else
                        {
                            //doesnt matter
                        }
                    }
                    coordXEnd--;
                    coordYEnd--;
                    coordXStart++;
                    coordYStart++;

                }

                //for (long y = coordYStart; y < coordYEnd; ++y)
                //{
                //    for(long x = coordXStart; x < coordXEnd; ++x)
                //    {
                //        if (grid[y][x] == '.')
                //        {
                //            finalArea = coord;
                //            foundAnswer = false;
                //            break;
                //        }
                //        else
                //        {
                //            //doesnt matter
                //        }
                //    }
                //}


                if (foundAnswer)
                {
                    break;
                }

            }


            Console.WriteLine($"Final Answer : {finalArea.coord1.ToString()} <-> {finalArea.coord2.ToString()} = Area: {finalArea.area}");

            result = finalArea.area;

            Console.WriteLine(coordinates.Count);

            //Console.WriteLine($"Min/Max coordidinates are:");
            //Console.WriteLine($"{minX},{minY}  ---  {maxX},{minY}  ");
            //Console.WriteLine($"{minX},{maxY}  ---  {maxX},{maxY}  ");


            //foreach (var row in grid)
            //{
            //    Console.WriteLine(new string(row));
            //}

            //Console.ReadLine();


            return result;
        }

        //private static void FillGrid()
        //{

        //    bool checking = true;

        //    int x = 0;
        //    int y = 0;

        //    Direction direction = Direction.Right;

        //    while (checking)
        //    {
        //        if (grid[y][x] == ' ')
        //        {
        //            grid[y][x] = '.';
        //        }


        //        if (direction == Direction.Right)
        //        {
        //            if (x >= grid[y].Length - 1)
        //            {
        //            }

        //            if (grid[y][x + 1] == ' ')
        //            {
        //                x++;
        //                continue;
        //            }
        //        }



        //    }

        //}



        //private static void FillNext(long x, long y)
        //{
        //    if (y < 0) return;
        //    if (x < 0) return;
        //    if (y > grid.Length - 1) return;
        //    if (x > grid[y].Length - 1) return;

        //    if (grid[y][x] == 'X' || grid[y][x] == '#' || grid[y][x] == '.')
        //    {
        //        return;
        //    }

        //    grid[y][x] = '.';

        //    FillNext(x + 1, y);

        //    FillNext(x, y + 1);

        //    FillNext(x - 1, y);

        //    FillNext(x, y - 1);
        //}


        private static double CalculateArea(EuclideanCoordinate coordinates1, EuclideanCoordinate coordinates2)
        {
            if (coordinates1 == null || coordinates2 == null || coordinates1.coordinateDimension == 0 || coordinates2.coordinateDimension == 0)
            {
                throw new ArgumentNullException("Coordinates were null or empty!");
            }

            if (coordinates1.coordinateDimension != coordinates2.coordinateDimension)
            {
                throw new ArgumentException($"Mismatched coordinateDimensions: {coordinates1.coordinateDimension} vs {coordinates2.coordinateDimension}");
            }

            if (coordinates1.coordinateDimension != 2)
            {
                throw new ArgumentException("Area can only be calculated for 2D coordinates.");
            }

            double length = Math.Abs(coordinates1.coordinates[0] - coordinates2.coordinates[0]) + 1;
            double width = Math.Abs(coordinates1.coordinates[1] - coordinates2.coordinates[1]) + 1;
            double area = length * width;
            return area;
        }


        private static double CalculateDistance(EuclideanCoordinate coordinates1, EuclideanCoordinate coordinates2)
        {
            double distance = 0;
            if (coordinates1 == null || coordinates2 == null || coordinates1.coordinateDimension == 0 || coordinates2.coordinateDimension == 0)
            {
                throw new ArgumentNullException("Coordinates were null or empty!");
            }

            if (coordinates1.coordinateDimension != coordinates2.coordinateDimension)
            {
                throw new ArgumentException($"Mismatched coordinateDimensions: {coordinates1.coordinateDimension} vs {coordinates2.coordinateDimension}");
            }



            for (int i = 0; i < coordinates1.coordinateDimension; ++i)
            {
                distance += Math.Pow(coordinates1.coordinates[i] - coordinates2.coordinates[i], 2);
            }

            distance = Math.Sqrt(distance);

            return distance;
        }

    }
}
