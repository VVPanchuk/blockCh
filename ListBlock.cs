using System;
using System.Collections.Generic;

namespace Test
{
    //создадим класс, который будет использовать класс Block
    public class ListBlock
    {
        //инициализируем список типизированый типом Block
        private static List<Block> blocksList = new List<Block>();
        //лок обьект
        private static Object thisLock = new Object();
        public static List<Block> BlocksList
        {
            get{lock (thisLock) {return blocksList;}}
            set{lock (thisLock){blocksList = value;}}
        }
        //метод добавления блока
        public static bool AddNewBlock()
        {
            ulong? sum = CheckBlocks(0, 0);
            if (sum == null)
                return false;
            
            BlocksList.Add(new Block((ulong)++sum));
            return true;
        }
        //метод проверки блока
        private static ulong? CheckBlocks(int i, ulong summ)
        {
            if (BlocksList.Count <= i)
                return summ;

            if (summ + 1 != BlocksList[i].Id)
            {
                return null;
            }
                summ += BlocksList[i].Id;
                return CheckBlocks(++i, summ);
            
        }
        public static void Show()
        {

                for (int i = 0; i < BlocksList.Count; i++)
                   Console.WriteLine($"{BlocksList[i].Id} : \t{BlocksList[i].Time} {BlocksList[i].Value}");
           
        }

    }
}
