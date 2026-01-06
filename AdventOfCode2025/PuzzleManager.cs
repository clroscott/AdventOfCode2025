using AdventOfCode.Domain.Common.IO;
using AdventOfCode.Infrastructure.Common.IO;
using AdventOfCode.Infrastructure.Tools.ButtonPressing;
using AdventOfCode.Infrastructure.Tools.Dimension;
using AdventOfCode.Infrastructure.Tools.Forklift;
using AdventOfCode.Infrastructure.Tools.Laser;
using AdventOfCode.Infrastructure.Tools.Lock;
using AdventOfCode.Infrastructure.Tools.PowerBank;
using AdventOfCode.Infrastructure.Tools.ProductID;
using AdventOfCode.Infrastructure.Tools.ServerRack;
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
                const bool part2 = true;

                List<long> finalValues = new List<long>();

                List<string> operations = new List<string>();

                foreach (var charVal in fileContent.Last())
                {
                    operations.Add(charVal.ToString());
                }

                if (part2)
                {
                    List<string> fakeFileContent = new List<string>();

                    var length = operations.Count;

                    List<long> values = new List<long>();
                    string currentString = "";
                    for (int i = length - 1; i >= 0; --i)
                    {
                        for (int j = 0; j < fileContent.Count; ++j)
                        {
                            if (j == fileContent.Count - 1)
                            {
                                values.Add(Convert.ToInt64(currentString.Trim()));
                                currentString = "";

                                if (fileContent[j][i] != ' ')
                                {

                                    for (int x = 0; x < values.Count; ++x)
                                    {
                                        long currentValue = values[x];
                                        if (x == 0)
                                        {
                                            finalValues.Add(currentValue);
                                        }
                                        else
                                        {

                                            switch (fileContent[j][i])
                                            {
                                                case '+':
                                                    Console.WriteLine(finalValues[finalValues.Count - 1] + " + " + currentValue);
                                                    finalValues[finalValues.Count - 1] += currentValue;
                                                    break;
                                                case '*':
                                                    Console.WriteLine(finalValues[finalValues.Count - 1] + " * " + currentValue);
                                                    finalValues[finalValues.Count - 1] *= currentValue;
                                                    break;
                                                case '-':
                                                    Console.WriteLine(finalValues[finalValues.Count - 1] + " - " + currentValue);
                                                    finalValues[finalValues.Count - 1] -= currentValue;
                                                    break;
                                                case '/':
                                                    Console.WriteLine(finalValues[finalValues.Count - 1] + " / " + currentValue);
                                                    finalValues[finalValues.Count - 1] /= currentValue;
                                                    break;
                                            }
                                        }
                                    }
                                    --i;
                                    values.Clear();
                                }
                            }
                            else
                            {
                                var val = fileContent[j][i];
                                currentString += val;
                            }
                        }

                    }

                }

                Console.WriteLine("Final Result pt2: " + finalValues.Sum());

                operations = fileContent.Last().Split(' ').ToList();
                operations.RemoveAll(i => i == "");
                finalValues.Clear();

                for (int i = 0; i < fileContent.Count - 1; ++i)
                {
                    var currentRow = fileContent[i].Split(' ').ToList();
                    currentRow.RemoveAll(i => i == "");

                    for (int j = 0; j < currentRow.Count; ++j)
                    {
                        long currentValue = Convert.ToInt64(currentRow[j]);

                        if (i == 0)
                        {
                            Console.WriteLine("Added to list: " + currentValue);
                            finalValues.Add(currentValue);
                        }
                        else
                        {

                            switch (operations[j])
                            {
                                case "+":
                                    Console.WriteLine(finalValues[j] + " + " + currentValue);
                                    finalValues[j] += currentValue;
                                    break;
                                case "*":
                                    Console.WriteLine(finalValues[j] + " * " + currentValue);
                                    finalValues[j] *= currentValue;
                                    break;
                                case "-":
                                    Console.WriteLine(finalValues[j] + " - " + currentValue);
                                    finalValues[j] -= currentValue;
                                    break;
                                case "/":
                                    Console.WriteLine(finalValues[j] + " / " + currentValue);
                                    finalValues[j] /= currentValue;
                                    break;
                            }

                            Console.WriteLine(finalValues[j]);
                        }
                    }

                }

                Console.WriteLine("Final Result pt1: " + finalValues.Sum());

            }
        }
        public static void Puzzle7(string filepath)
        {
            IFileService fileService = new FileService();
            var fileContent = fileService.ReadLines(filepath).Result;

            if (fileContent is null || fileContent.Count <= 0)
            {
                Console.WriteLine("File is null or empty");
            }
            else
            {
                var result = LaserManger.countSplits(fileContent);

                Console.WriteLine("Num splits: " + result);
            }
        }
        public static void Puzzle8(string filepath)
        {
            IFileService fileService = new FileService();
            var fileContent = fileService.ReadLines(filepath).Result;

            if (fileContent is null || fileContent.Count <= 0)
            {
                Console.WriteLine("File is null or empty");
            }
            else
            {
                var result = EuclideanCalculator.ChainBuilding(fileContent);

                Console.WriteLine($"result : {result}");
            }
        }
        public static void Puzzle9(string filepath)
        {
            IFileService fileService = new FileService();
            var fileContent = fileService.ReadLines(filepath).Result;

            if (fileContent is null || fileContent.Count <= 0)
            {
                Console.WriteLine("File is null or empty");
            }
            else
            {
                var result = EuclideanCalculator.RectangleFinder(fileContent);

                Console.WriteLine($"result : {result}");
            }
        }
        public static void Puzzle10(string filepath)
        {
            IFileService fileService = new FileService();
            var fileContent = fileService.ReadLines(filepath).Result;

            if (fileContent is null || fileContent.Count <= 0)
            {
                Console.WriteLine("File is null or empty");
            }
            else
            {
                var result = ButtonMachineCalculator.CaculatePushes(fileContent);

                Console.WriteLine($"result : {result}");
            }
        }
        public static void Puzzle11(string filepath)
        {
            IFileService fileService = new FileService();
            var fileContent = fileService.ReadLines(filepath).Result;

            if (fileContent is null || fileContent.Count <= 0)
            {
                Console.WriteLine("File is null or empty");
            }
            else
            {
                var result = ServerRackCalculator.CalculateYouToOut(fileContent);

                Console.WriteLine($"result : {result}");
            }
        }
        public static void Puzzle12(string filepath)
        {
            IFileService fileService = new FileService();
            var fileContent = fileService.ReadLines(filepath).Result;

            if (fileContent is null || fileContent.Count <= 0)
            {
                Console.WriteLine("File is null or empty");
            }
            else
            {
                var result = "";// ButtonMachineCalculator.CaculatePushes(fileContent);

                Console.WriteLine($"result : {result}");
            }
        }
    }
}