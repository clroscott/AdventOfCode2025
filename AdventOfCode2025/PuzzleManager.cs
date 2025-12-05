using AdventOfCode.Domain.Common.IO;
using AdventOfCode.Infrastructure.Common.IO;
using AdventOfCode.Infrastructure.Tools.Forklift;
using AdventOfCode.Infrastructure.Tools.Lock;
using AdventOfCode.Infrastructure.Tools.PowerBank;
using AdventOfCode.Infrastructure.Tools.ProductID;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2025
{
    public static class PuzzleManager
    {

        public static void Puzzle1(string filepath)
        {
            //string filepath = "C:\\Users\\clayb\\source\\repos\\AdventOfCode2025\\AdventOfCode2025\\Puzzles\\puzzle1.txt"
            IFileService fileService = new FileService();

            var fileContent = fileService.ReadLines(filepath).Result;

            List<int> positionsVisited = new List<int>();

            if (fileContent is null || fileContent.Count <= 0)
            {
                Console.WriteLine("File is null or empty");
            }
            else
            {
                const int startingPosition = 50;
                ILockDial lockDial = new LockDial(startingPosition);

                foreach (var line in fileContent)
                {
                    Direction dir = Direction.Right;
                    int steps = 0;

                    if (line.StartsWith("R") || line.StartsWith("r"))
                    {
                        dir = Direction.Right;

                    }
                    else if (line.StartsWith("L") || line.StartsWith("l"))
                    {
                        dir = Direction.Left;
                    }
                    else
                    {
                        throw new Exception("Invalid direction in input file.");
                    }


                    steps = int.Parse(line.Replace("R", "").Replace("r", "").Replace("L", "").Replace("l", "").Trim());

                    lockDial.Turn(dir, steps);
                    positionsVisited.Add(lockDial.CurrentPosition);

                    Console.WriteLine($"{dir} {steps} to {lockDial.CurrentPosition}. Passed zero: {lockDial.PassedZeroCount}");

                }

                //foreach (var pos in positionsVisited)
                //{
                //    Console.WriteLine($"Visited position: {pos}");
                //}

                Console.WriteLine($"0 Count: {positionsVisited.Where(i => i == 0).ToList().Count}");
                Console.WriteLine($"PassedZeroCount: {lockDial.PassedZeroCount}");
            }

        }
        public static void Puzzle2(string filepath)
        {
            IFileService fileService = new FileService();
            IProductFinder finder = new ProductFinder();

            var fileContent = fileService.ReadFile(filepath).Result;

            long sum = 0;

            if (string.IsNullOrEmpty(fileContent))
            {
                Console.WriteLine("File is null or empty");
            }
            else
            {
                var ranges = fileContent.Split(',').ToList();

                foreach (var range in ranges)
                {
                    var startEnd = range.Split('-');

                    Console.Write($"Range: {range}");
                    if (startEnd.Length != 2)
                    {
                        Console.WriteLine($" -- Invalid range");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine();
                    }

                    var start = long.Parse(startEnd[0].Trim());
                    var end = long.Parse(startEnd[1].Trim());

                    for (long i = start; i <= end; ++i)
                    {
                        bool isValid = true;

                        //int digitRepeat = 2;
                        for (int j = 2; j <= i.ToString().Length; ++j)
                        {
                            isValid = finder.IsValidProductID(i.ToString(), j);
                            if (!isValid)
                            {
                                break;
                            }
                        }


                        if (!isValid)
                        {
                            Console.Write(i);
                            Console.WriteLine($" -- {isValid}");
                            sum += i;
                        }
                    }
                }
            }
            Console.WriteLine("");
            Console.WriteLine($"Sum of invalid product IDs: {sum}");
        }

        public static void Puzzle3(string filepath)
        {
            IFileService fileService = new FileService();
            IPowerBank powerBank = new PowerBank();
            var fileContent = fileService.ReadLines(filepath).Result;
            if (fileContent is null || fileContent.Count <= 0)
            {
                Console.WriteLine("File is null or empty");
            }
            else
            {

                long sumPower = 0;

                foreach (var line in fileContent)
                {
                    Console.WriteLine(line);
                    var result = powerBank.JolatgeOutput(line, 12);

                    sumPower += result;

                    Console.WriteLine($"Joltage Output: {result}");
                }
                Console.WriteLine();
                Console.WriteLine($"Total Joltage Output: {sumPower}");
            }
        }
        public static void Puzzle4(string filepath)
        {
            IFileService fileService = new FileService();
            var fileContent = fileService.ReadLines(filepath).Result;
            Forklift fork = new Forklift();

            if (fileContent is null || fileContent.Count <= 0)
            {
                Console.WriteLine("File is null or empty");
            }
            else
            {
                int result = fork.RollCount(fileContent, 4, 1);

                Console.WriteLine(result);
            }
        }

        public static void Puzzle5(string filepath)
        {
            IFileService fileService = new FileService();
            var fileContent = fileService.ReadLines(filepath).Result;

            if (fileContent is null || fileContent.Count <= 0)
            {
                Console.WriteLine("File is null or empty");
            }
            else
            {
                IProductFinder finder = new ProductFinder();
                int blankIndex = fileContent.IndexOf("");

                var ranges = fileContent.GetRange(0, blankIndex);
                fileContent.RemoveRange(0, blankIndex + 1);

                var count = finder.FreshCounter(ranges, fileContent);

                Console.WriteLine($"Fresh products in list : {count.Item1}. Total Potential Fresh: {count.Item2}");
            }
        }
        public static void Puzzle6(string filepath)
        {
            IFileService fileService = new FileService();
            var fileContent = fileService.ReadLines(filepath).Result;

            if (fileContent is null || fileContent.Count <= 0)
            {
                Console.WriteLine("File is null or empty");
            }
            else
            {

            }
        }
    }
}