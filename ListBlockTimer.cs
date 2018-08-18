using System;
using System.Collections.Generic;

namespace Test
{
    public class ListBlockTimer
    {
        //
        private  List<Block> blocksList = new List<Block>();
        private  Object thisLock = new Object();
        public  List<Block> BlocksList
        {
            get
            {
                lock (thisLock)
                {
                    return blocksList;
                }
            }
            set
            {
                lock (thisLock)
                {
                    blocksList = value;
                }
            }
        }
        public void RunCreatingBlocks(Object stopTimerActiom)
        {
            Action stopTimer = (Action)stopTimerActiom;
            if (!AddNewBlock())
            {
                var cur = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("there was an out of sync");
                Console.ForegroundColor = cur;

                stopTimer?.Invoke();
            }
        }
        public  bool AddNewBlock()
        {
            ulong? sum = CheckBlocks(0, 0);
            if (sum == null)
                return false;

            BlocksList.Add(new Block((ulong)++sum));
            return true;
        }

        private ulong? CheckBlocks(int i, ulong summ)
        {
            if (BlocksList.Count <= i)
                return summ;

            if (summ + 1 != BlocksList[i].Id)
            {
                return null; // or -1
            }
            summ += BlocksList[i].Id;
            return CheckBlocks(++i, summ);

        }
        public  void Show()
        {

            for (int i = 0; i < BlocksList.Count; i++)
                Console.WriteLine($"{BlocksList[i].Id} : \t{BlocksList[i].Time} {BlocksList[i].Value}");

        }

       
    }
}
