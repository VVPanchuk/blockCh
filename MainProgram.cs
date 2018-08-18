using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Test
{
    public enum EnumCommand
    {
        take,
        count,
        change
    }
    
    public class MainProgram
    {
        private Action StopTimer ;
        private Dictionary<EnumCommand, Action<string[]>> CommandDictionary =
            new Dictionary<EnumCommand, Action<string[]>>();
        Timer timer;
        private ListBlockTimer blockCtrl;

        public MainProgram()
        {
            CommandDictionary.Add(EnumCommand.take, CommandTake);
            CommandDictionary.Add(EnumCommand.count, CommandCount);
            CommandDictionary.Add(EnumCommand.change, CommandChange);
            StopTimer += OnStopTimer;
        }

        private void OnStopTimer()
        {
            timer.Dispose();
            timer = null;
            Console.WriteLine("timer dispose");
            if (StopTimer != null)
                StopTimer -= OnStopTimer;
        }

        public void Run()
        {
            blockCtrl = new ListBlockTimer();
            timer = new Timer(blockCtrl.RunCreatingBlocks, StopTimer, 0, 1000);
           
            EnumCommand commandValue;

            while (timer != null)
            {
                Menu();
                var input = Console.ReadLine().ToLower();
                string[] inputCommands = input.Split(' ').ToArray();
                
                try
                {
                    commandValue = (EnumCommand) Enum.Parse(typeof (EnumCommand), inputCommands[0]);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine($"{input} is not a member of the Command enumeration.");
                    continue;
                }
                
                if (CommandDictionary.ContainsKey(commandValue))
                    CommandDictionary[commandValue](inputCommands);

                Console.WriteLine("========");
            }
            Console.ReadLine();
        }


        private void CommandChange(string[] input)
        {
            if (input == null || input.Length < 2)
            {
                Console.WriteLine("you need correct input blok# and new id");
                return;
            }
            int blockNumber;
            ulong newId;
            if (!int.TryParse(input[1], out blockNumber))
            {
                Console.WriteLine($"cannot understand block number: {input[1]}");
                return;
            }
            if (!ulong.TryParse(input[2], out newId))
            {
                Console.WriteLine($"cannot understand new id: {input[2]}");
                return;
            }
            if (blockCtrl.BlocksList.Count - 1 < blockNumber)
            {
                Console.WriteLine($"max is {blockCtrl.BlocksList.Count - 1}");
                return;
            }

            blockCtrl.BlocksList[blockNumber].Id = newId;
            var cur = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("done");
            Console.ForegroundColor = cur;


        }

        private void CommandCount(string[] input)
        {
            Console.WriteLine($" we have: {blockCtrl.BlocksList.Count}");
        }

        private void CommandTake(string[] input)
        {
            int number;
            if (input == null || input.Length < 2)
            {
                Console.WriteLine("you need correct input blok#");
                return;
            }
            if (!int.TryParse(input[1], out number))
            {
                Console.WriteLine($"incorrect number {input[1]}");
                return;
            }

            if (blockCtrl.BlocksList.Count - 1 < number)
            {
                Console.WriteLine($"max is {blockCtrl.BlocksList.Count - 1}");
                return;
            }

            var bloc = blockCtrl.BlocksList[number];
            Console.WriteLine($"block #{number}\nid:\t{bloc.Id} \ntime:\t{bloc.Time}\nGuid:\t{bloc.Value}");



        }
        private void Menu()
        {
            var cur = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Input Command:\n. take {block number}\n. count\n. change {block number} {new id}");
            Console.ForegroundColor = cur;
        }
    }
}

