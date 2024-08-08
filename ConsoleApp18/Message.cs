using System.Text.Json;

namespace ConsoleApp18
{
    public class Message
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public DateTime Time { get; set; }

        public Message(string nik, string text)
        {
            Name = nik;
            Text = text;
            Time = DateTime.Now;
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static Message? FromJson(string some)
        {
            return JsonSerializer.Deserialize<Message>(some);
        }

        public Message()
        {

        }

        public override string ToString()
        {
            return $"Получено сообщение от {Name}({Time.ToString()})\n{Text}";
        }

    }
}
