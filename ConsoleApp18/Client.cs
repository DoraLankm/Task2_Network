using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    internal class Client
    {
        public static void SendMsg(string nik)
        {
            while (true)
            {
                Console.WriteLine("Введите сообщение");
                string text = Console.ReadLine();
                if (text == "Exit")
                {
                    break;
                }
                Message message = new Message(nik, text);
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
                UdpClient ucl = new UdpClient();
                string json = message.ToJson();
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                ucl.Send(bytes, iPEndPoint);

                byte[] buffer = ucl.Receive(ref iPEndPoint);
                string str = Encoding.UTF8.GetString(buffer);
                Message? ms = Message.FromJson(str);
                if (ms != null)
                {
                    Console.WriteLine(ms.ToString());
                }
            }

        }
    }
}
