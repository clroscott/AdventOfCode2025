using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace AdventOfCode.Infrastructure.Tools.Forklift
{
    public class Forklift
    {
        readonly char rollChar;
        readonly char rollSuccessChar;
        readonly char rollEmptyChar;
        readonly char[] validChars;

        public Forklift()
        {
            rollChar = '@';
            rollSuccessChar = 'X';
            rollEmptyChar = '.';
            this.validChars = new char[] { rollChar, rollSuccessChar };
        }

        public Forklift(char rollChar, char rollSuccessChar, char rollEmptychar)
        {
            this.rollChar = rollChar;
            this.rollSuccessChar = rollSuccessChar;
            this.rollEmptyChar = rollEmptychar;
            this.validChars = new char[] { rollChar, rollSuccessChar };
        }

        public int RollCount(List<string> fileRows, int numAdjacentRolls, int surroundingLength)
        {


            var grid = CreateGrid(fileRows);

            Console.WriteLine("------First Grid Start------");
            PrintGrid(grid);
            Console.WriteLine("------First Grid End------");

            int result = 0;
            int previousTotal = -1;//intial Value to ensure enters the loop

            while (previousTotal != result)
            {
                previousTotal = result;

                //Parallel.For(0, grid.Length, y =>
                for (int y = 0; y < grid.Length; ++y)
                {
                    for (int x = 0; x < grid[y].Length; ++x)
                    {
                        char currentRoll = grid[y][x];

                        Console.Write(currentRoll);

                        if (currentRoll != rollChar)
                        {
                            continue;
                        }
                        else
                        {
                            List<char> foundChars = new List<char>();
                            //for (int i = 1; i <= surroundingLength; ++i)
                            //{

                            //}

                            char value1 = '.';
                            char value2 = '.';
                            char value3 = '.';

                            char value4 = '.';
                            char value5 = '.';

                            char value6 = '.';
                            char value7 = '.';
                            char value8 = '.';

                            //x-1 y-1, x y-1, x+1 y-1
                            //x-1 y , x y , x+1 y 
                            //x-1 y+1, x, y+1, x+1 y+1

                            //if (x - 1 >= 0 && y - 1 >= 0) value1 = grid[x - 1][y - 1];
                            //if (y - 1 >= 0) value2 = grid[x][y - 1];
                            //if (x + 1 < grid[y].Length - 1 && y - 1 >= 0) value3 = grid[x + 1][y - 1];

                            //if (x - 1 >= 0) value4 = grid[x - 1][y];
                            //if (x + 1 < grid[y].Length - 1) value5 = grid[x + 1][y];

                            //if (x - 1 >= 0 && y < grid.Length - 1) value6 = grid[x - 1][y + 1];
                            //if (y < grid.Length - 1) value7 = grid[x][y + 1];
                            //if (x + 1 < grid[y].Length - 1 && y < grid.Length - 1) value8 = grid[x + 1][y + 1];




                            //y-1 x-1, y-1 x , y-1 x+1 
                            //y x-1, y x  , y x+1 
                            //y+1 x-1, x y+1, y+1 x+1 


                            if (y - 1 >= 0)
                            {
                                if (x - 1 >= 0)
                                {
                                    value1 = grid[y - 1][x - 1];
                                }

                                value2 = grid[y - 1][x + 0];

                                if (x + 1 < grid[0].Length)
                                {
                                    value3 = grid[y - 1][x + 1];
                                }
                            }

                            if (x - 1 >= 0)
                            {
                                value4 = grid[y + 0][x - 1];
                            }
                            //valueX = grid[y + 0][x + 0];

                            if (x + 1 < grid[0].Length)
                            {
                                value5 = grid[y + 0][x + 1];
                            }

                            if (y + 1 < grid.Length)
                            {
                                if (x - 1 >= 0)
                                {
                                    value6 = grid[y + 1][x - 1];
                                }
                                value7 = grid[y + 1][x + 0];

                                if (x + 1 < grid[0].Length)
                                {
                                    value8 = grid[y + 1][x + 1];
                                }
                            }



                            foundChars.Add(value1);
                            foundChars.Add(value2);
                            foundChars.Add(value3);
                            foundChars.Add(value4);
                            foundChars.Add(value5);
                            foundChars.Add(value6);
                            foundChars.Add(value7);
                            foundChars.Add(value8);


                            if (foundChars.Where(i => validChars.Contains(i)).Count() < numAdjacentRolls)
                            {
                                grid[y][x] = rollEmptyChar;
                                ++result;
                            }
                        }
                    }
                    Console.WriteLine();
                }
            }


            Console.WriteLine("------Second Grid Start------");
            PrintGrid(grid);
            Console.WriteLine("------Second Grid End------");


            return result;
        }

        private void PrintGrid(char[][] grid)
        {
            for (int i = 0; i < grid.Length; ++i)
            {

                for (int j = 0; j < grid[i].Length; ++j)
                {
                    Console.Write(grid[i][j]);
                }
                Console.WriteLine();
            }
        }


        private char[][] CreateGrid(List<string> fileRows)
        {
            char[][] grid = new char[fileRows.Count][];

            for (int i = 0; i < fileRows.Count; ++i)
            {
                grid[i] = fileRows[i].ToArray();
            }
            return grid;
        }
    }
}
