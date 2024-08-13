using System.Text.Json;

namespace ConsoleApp18
{

    public class Message
    {
        public string FromName { get; set; }
        public string Text { get; set; }

        public string ToName { get; set; }
        public DateTime Time { get; set; }
 


        public Message(string nik, string text)
        {
            FromName = nik;
            Text = text;
            Time = DateTime.Now;
        }


        public Message()
        {

        }

        public override string ToString()
        {
            return $">> To get a message from {FromName}({Time.ToString()})\n>>{Text}";
        }

    }
}
