using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Infrastructure.Tools.Laser
{
    public class MoveResult
    {
        public long SplitCount { get; set; }
        public long EndCount { get; set; }
    }

    public class LaserManger
    {
        public static char[][] grid;
        public static long[][] gridCount;
        public static readonly object lockObject = new object();

        public static Tuple<long, long> countSplits(List<string> fileContents)
        {
            //returnValue1 = 0;
            //returnValue2 = 0;
            MoveResult result = new MoveResult();

            grid = new char[fileContents.Count][];

            gridCount = new long[fileContents.Count][];

            for (int i = 0; i < fileContents.Count; i++)
            {
                grid[i] = fileContents[i].ToCharArray();
                gridCount[i] = new long[fileContents[i].Length];
            }

            Console.WriteLine("Start: ");
            Console.WriteLine("");
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; ++j)
                {
                    char c = grid[i][j];
                    gridCount[i][j] = -1;
                    Console.Write(c);
                }
                Console.WriteLine();
            }

            for (int i = 0; i < fileContents[0].Length; ++i)
            {
                if (fileContents[0][i] == 'S')
                {
                    result = Move(1, i);
                    break;
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Done: ");
            Console.WriteLine("");

            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; ++j)
                {
                    char c = grid[i][j];
                    Console.Write(c);
                }
                Console.WriteLine();
            }


            return new Tuple<long, long>(result.SplitCount, result.EndCount);
        }

        private static MoveResult Move(int row, int col)
        {

            MoveResult result = new MoveResult { SplitCount = 0, EndCount = 0 };

            if (row >= grid.Length)
            {
                //returnValue2++;
                //Console.WriteLine(returnValue1);

                return new MoveResult { EndCount = 1 };
            }

            if (gridCount[row][col] != -1)
            {
                return new MoveResult { EndCount =  gridCount[row][col] };
            }

            if (grid[row][col] == '.' || grid[row][col] == '|')
            {
                grid[row][col] = '|';
                result = Move(row + 1, col);
            }
            else if (grid[row][col] == '^' || grid[row][col] == 'v')
            {
                if (grid[row][col] == '^')
                {
                    lock (lockObject)
                    {
                        if (grid[row][col] == '^')
                        {
                            result.SplitCount += 1;
                        }
                    }
                    //returnValue1++;
                    //Console.WriteLine();
                    //Console.WriteLine("returnValue1 : " + returnValue1);
                }
                grid[row][col] = 'v';

                #region tasked
                //var leftTask = Task.Run(() => Move(row, col - 1));

                //var rightTask = Task.Run(() => Move(row, col + 1));

                //Task.WhenAll(leftTask, rightTask);

                //MoveResult leftResult = leftTask.Result;
                //MoveResult rightResult = rightTask.Result;


                //result.SplitCount += leftResult.SplitCount + rightResult.SplitCount;
                //result.EndCount += leftResult.EndCount + rightResult.EndCount;

                #endregion tasked

                #region Not Tasked
                

                MoveResult leftResult = Move(row, col - 1);
                MoveResult rightResult = Move(row, col + 1);


                result.SplitCount += leftResult.SplitCount + rightResult.SplitCount;
                result.EndCount += leftResult.EndCount + rightResult.EndCount;
                #endregion Not Tasked


                //Console.WriteLine($"{row}, {col} : {result.SplitCount}, {result.EndCount}");
            }
            gridCount[row][col] = result.EndCount;

            return result;
        }


        public static Tuple<long, long> countSplitsv1(List<string> fileContents)
        {
            long returnValue1 = 0;
            long returnValue2 = 0;


            for (int i = 0; i < fileContents.Count; ++i)
            {
                int length = fileContents[i].Length;
                for (int j = 0; j < length; ++j)
                {
                    var currentChar = fileContents[i][j];

                    var nextRow = fileContents[i].ToArray();

                    if (i > length - 1)
                    {
                        nextRow = new char[0];
                    }
                    else
                    {
                        nextRow = fileContents[i + 1].ToArray();
                    }
                    //var nextRowAfter = j == length - 1 ? new char[0] : fileContents[i + 2].ToArray();


                    //var nextRowAfter = nextRow;

                    //if (i > length - 2)
                    //{
                    //    nextRowAfter = new char[0];
                    //}
                    //else
                    //{
                    //    nextRowAfter = fileContents[i + 2].ToArray();
                    //}

                    if (currentChar == 'S')
                    {
                        nextRow[j] = '|';

                    }
                    else if (currentChar == '|')
                    {
                        if (nextRow.Length > 0)
                        {
                            if (nextRow[j] == '.')
                            {
                                nextRow[j] = '|';
                            }
                            else if (nextRow[j] == '^')
                            {

                                returnValue1++;
                                nextRow[j - 1] = '|';
                                nextRow[j + 1] = '|';
                                //if (nextRowAfter.Length > 0)
                                //{
                                //    nextRowAfter[j - 1] = '|';
                                //    nextRowAfter[j + 1] = '|';
                                //}
                            }
                        }
                    }

                    if (nextRow.Length > 0)
                    {
                        fileContents[i + 1] = new string(nextRow);
                    }
                    //if (nextRowAfter.Length > 0)
                    //{
                    //    fileContents[i + 2] = new string(nextRowAfter);
                    //}
                }
                //Console.WriteLine(i + ": " + fileContents[i]);

                Console.WriteLine();
                Console.WriteLine(i);
                for (int x = 0; x < fileContents.Count; ++x)
                {
                    Console.WriteLine(x + ": " + fileContents[x]);
                }
            }

            Console.WriteLine();
            for (int i = 0; i < fileContents.Count; ++i)
            {
                if (fileContents[i].Contains('^'))
                {
                    returnValue2 += fileContents[i].Where(x => x.Equals('|')).Count();
                }

                Console.WriteLine(i + ": " + fileContents[i]);
            }

            return new Tuple<long, long>(returnValue1, returnValue2);
        }
    }
}
