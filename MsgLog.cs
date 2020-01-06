using System;

namespace WPF_SomeName200_bot
{
    /// <summary>
    /// Класс для создания объектов лога переписки
    /// </summary>
    public class MsgLog
    {
        public string Time { get; set; }
        public long ID { get; set; }
        public string Msg { get; set; }
        public string Name { get; set; }

        public MsgLog(string msg, string firstName, long id)
        {
            Time = DateTime.Now.ToLongTimeString();
            ID = id;
            Msg = msg;
            Name = firstName;
        }

        public override string ToString()
        {
            return $"{DateTime.Now.ToLongTimeString()} {Msg} {Name} {ID}\n";
        }
    }
}
