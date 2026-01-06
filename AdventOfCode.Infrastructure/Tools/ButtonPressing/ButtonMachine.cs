using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AdventOfCode.Infrastructure.Tools.ButtonPressing
{

    public static class ButtonMachineCalculator
    {
        public static long CaculatePushes(List<string> files)
        {

            bool doPuzzle1 = true;

            long result = 0;

            List<ButtonMachine> machines = new List<ButtonMachine>();

            Console.WriteLine("Parsing");
            foreach (var fileRow in files)
            {
                machines.Add(ButtonMachineFactory.Create(fileRow));
            }

            Console.WriteLine("Parsed!");

            if (doPuzzle1)
            {
                result = Puzzle1(result, machines);
            }


            foreach (var machine in machines)
            {

                List<List<List<Button>>> values = new List<List<List<Button>>>();

                var joltages = machine.targetJoltage.OrderBy(x => x.value).ToList();

                //int combinationCount =

                //List < List < Button >> allList = new List<List<Button>>();

                //bool fillingGrid = true;
                //while (fillingGrid)
                //{




                for (int index = 0; index < joltages.Count; ++index)
                {
                    var buttons = machine.buttons.Where(x => x.lightsPushed.Contains(joltages[index].index)).ToList();

                    List<List<int>> pushes = new List<List<int>>();


                    List<int> pushesToAdd = new List<int>();

                    foreach (var button in buttons)
                    {
                        pushesToAdd.Add(0);
                    }

                    pushesToAdd[0] = joltages[index].value;

                    pushes.Add(new List<int>(pushesToAdd));


                    bool filling = true;

                    while(filling)
                    {

                        for (int i = 0; i < pushesToAdd.Count; ++i)
                        {

                            pushesToAdd[i]--;
                            pushesToAdd[i + 1]++;
                            pushes.Add(new List<int>(pushesToAdd));
                        
                        }


                        if (pushes.Count > 15)
                        {
                            filling = false;
                        }
                    }



                }

                //for (int x = 0; x < buttons.Count; x++)
                //{

                //    List<Button> bList = new List<Button>();
                //    for (int i = 0; i < joltages[index].value; ++i)
                //    {
                //        bList.Add(buttons[x]);
                //    }
                //    allList.Add(bList);
                //}


                //}

            }



            return result;
        }

        private static long Puzzle1(long result, List<ButtonMachine> machines)
        {
            Dictionary<int, List<List<List<Button>>>> potentialCombos = new Dictionary<int, List<List<List<Button>>>>();
            //Dictionary<int, List<List<string>>> potentialCombos = new Dictionary<int, List<List<string>>>();

            foreach (var machine in machines)
            {
                List<List<List<Button>>> values = new List<List<List<Button>>>();

                if (potentialCombos.ContainsKey(machine.buttons.Count))
                {
                    machine.buttonCombinations = potentialCombos[machine.buttons.Count];
                    continue;
                }


                for (int index = 0; index < machine.buttons.Count; ++index)
                //while (values.Count < machines[0].buttons.Count)
                {
                    if (values.Count == 0)
                    {
                        values.Add(new List<List<Button>>());
                        for (int i = 0; i < machine.buttons.Count; ++i)
                        {
                            values[index].Add(new List<Button> { machine.buttons[i] });
                        }
                    }
                    else
                    {
                        values.Add(new List<List<Button>>());
                        for (int i = 0; i < values[index - 1].Count - 1; ++i)
                        {
                            var curval = values[index - 1];
                            var curthing = curval[i];
                            int start = machine.buttons.IndexOf(curthing.Last());
                            //var charthing = curthing.Last().ToString();
                            //int start = Convert.ToInt32(charthing);



                            for (int j = start + 1; j < machine.buttons.Count; ++j)
                            {
                                List<Button> newValues = new List<Button>();
                                newValues.AddRange(values[index - 1][i]);
                                newValues.Add(machine.buttons[j]);
                                values[index].Add(newValues);

                                //values[index].Add(values[index - 1][i] + j.ToString());
                            }
                        }
                    }

                }
                machine.buttonCombinations = values;
                potentialCombos.Add(machine.buttons.Count, values);

            }

            foreach (var machine in machines)
            {
                Console.WriteLine("---");
                Console.WriteLine(machine.ToString());

                bool foundCombo = false;
                long buttonPushed = 9999;
                foreach (var comboList in machine.buttonCombinations)
                {
                    foreach (var combo in comboList)
                    {
                        //var buttons = combo.ToCharArray();
                        foreach (var button in combo)
                        {
                            machine.PushButton(button);
                            //var buttonIndex = Convert.ToInt32(button.ToString());

                            //machine.PushButton(buttonIndex);
                        }
                        if (machine.currentLights.SequenceEqual(machine.targetLights))
                        {
                            Console.WriteLine();
                            Console.WriteLine(string.Join(',', combo));
                            Console.WriteLine();
                            foundCombo = true;
                            buttonPushed = combo.Count;
                            break;
                        }
                        machine.ResetMachine();
                    }
                    if (foundCombo)
                    {
                        break;
                    }
                }
                result += buttonPushed;

                Console.WriteLine(machine.ToString());
            }

            return result;
        }
    }


    public class ButtonMachine
    {
        private List<char> _targetLights;

        private List<(int index, int value)> _targetJoltage;


        public List<char> targetLights
        {
            get
            {
                return _targetLights;
            }
        }
        public List<char> currentLights { get; set; }

        public List<Button> buttons { get; set; }
        public List<(int index, int value)> targetJoltage
        {
            get
            {
                return _targetJoltage.OrderBy(x => x.index).ToList();
            }
        }

        public List<int> currentJoltage { get; set; }

        public List<List<List<Button>>> buttonCombinations { get; set; }

        public ButtonMachine(List<char> target, List<Button> buttons, List<int> joltage)
        {
            _targetLights = target;
            this.buttons = buttons;
            _targetJoltage = new List<(int index, int value)>();
            for (int i = 0; i < joltage.Count; ++i)
            {
                _targetJoltage.Add(new(i, joltage[i]));
            }

            ResetMachine();
        }

        public void ResetMachine()
        {
            ResetCurrentLights();
            ResetCurrentJoltage();
        }

        public void PushButton(int index)
        {
            var button = buttons[index];

            PushButton(button);

        }

        public void PushButton(Button button)
        {
            foreach (var light in button.lightsPushed)
            {
                if (currentLights[light] == '.')
                {
                    currentLights[light] = '#';
                }
                else
                {
                    currentLights[light] = '.';
                }

                currentJoltage[light]++;
            }
        }

        public void ResetCurrentLights()
        {
            this.currentLights = new List<char>();
            for (int i = 0; i < targetLights.Count; i++)
            {
                currentLights.Add('.');
            }
        }
        public void ResetCurrentJoltage()
        {

            this.currentJoltage = new List<int>();

            for (int i = 0; i < targetJoltage.Count; i++)
            {
                currentJoltage.Add(0);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Current: " + new string(currentLights.ToArray()));
            sb.Append("\n");
            sb.Append("Target: " + new string(targetLights.ToArray()));
            sb.Append("\n");

            sb.Append("Buttons: ");

            foreach (var button in buttons)
            {
                sb.Append("(");

                foreach (var push in button.lightsPushed)
                {
                    sb.Append(push + ",");
                }

                sb.Length--;
                sb.Append(")");
            }



            sb.Append("\n");
            sb.Append("Current Joltage: {" + string.Join(',', currentJoltage.ToArray()) + "}");
            sb.Append("\n");
            sb.Append("Target Joltage: {" + string.Join(',', targetJoltage.ToArray()) + "}");

            return sb.ToString();

        }

    }

    [DebuggerDisplay("{DebuggerValues}")]
    public class Button
    {
        public List<int> lightsPushed { get; set; } = new List<int>();

        private string DebuggerValues =>
        string.Join(",", lightsPushed);
    }


    //[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
    //[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
    //[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}


    public static class ButtonMachineFactory
    {
        public static ButtonMachine Create(string fileRow)
        {
            List<char> targetLights;
            List<Button> buttons = new List<Button>();
            List<int> joltage = new List<int>();


            var fileSplit = fileRow.Split(' ');

            targetLights = fileSplit[0].Trim('{').Trim('}').Trim('[').Trim(']').ToCharArray().ToList();

            fileSplit[fileSplit.Length - 1].Trim('{').Trim('}').Split(',').ToList()
                .ForEach(i =>
                {
                    joltage.Add(Convert.ToInt32(i));
                });


            for (int i = 1; i < fileSplit.Length - 1; ++i)
            {
                Button button = new Button();
                foreach (var item in fileSplit[i].Trim('(').Trim(')').Split(','))
                {
                    button.lightsPushed.Add(Convert.ToInt32(item));
                }

                buttons.Add(button);
            }


            return new ButtonMachine(targetLights, buttons, joltage);
        }
    }
}
