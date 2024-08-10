using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    internal class Client
    {
        public static async Task SendMsg(string nik)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            using UdpClient ucl = new UdpClient();

            while (true)
            {
                Console.WriteLine("Введите сообщение");
                string text = Console.ReadLine();
                if (text == "Exit")
                {
                    await ucl.SendAsync(Encoding.UTF8.GetBytes(text), iPEndPoint);
                    break;
                }
                else
                {
                    Message message = new Message(nik, text);
                    string json = message.ToJson();
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    await ucl.SendAsync(bytes, iPEndPoint);

                    var result = await ucl.ReceiveAsync();
                    string str = Encoding.UTF8.GetString(result.Buffer);
                    Message? ms = Message.FromJson(str);
                    if (ms != null)
                    {
                        Console.WriteLine(ms.ToString());
                    }
                }

                
            }
        }
    }
}