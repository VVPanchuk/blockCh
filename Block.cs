using System;

namespace Test
{
    //пройстой блок
    public class Block
    {
        //при создании блока нужно передать ему уникальный ид и в конструкторе задать параметры 
        public Block(ulong id)
        {
            Id = id;
            Time = DateTime.Now;
            Value = Guid.NewGuid().ToString();
        }

        public ulong Id { get; set; }

        public DateTime Time { get; }

        public string Value { get; }
    }
}
