using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    internal class Server
    {

        public static void Start()
        {
            Thread thread = new(() => {AcceptMs(); });
            thread.IsBackground = true;
            thread.Start();
            Console.ReadKey();
        }
       public static void AcceptMs()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
            UdpClient ucl = new UdpClient(12345);
            Console.WriteLine("Сервер ожидает сообщения от клиента");
            while (true)
            {
                
                try
                {
                    byte[] buffer = ucl.Receive(ref iPEndPoint);
                    string str = Encoding.UTF8.GetString(buffer);


                    Thread thread = new Thread(() =>
                    {
                        Message? message = Message.FromJson(str);

                        if (message != null)
                        {
                            Console.WriteLine(message.ToString());
                            Message ms = new Message("Server", "Сообщение отправлено");
                            string json = ms.ToJson();
                            byte[] bytes = Encoding.UTF8.GetBytes(json);
                            ucl.Send(bytes, iPEndPoint);
                        }

                        else
                        {
                            Console.WriteLine("Некорректное сообщение");
                        }

                    });
                    thread.Start();


                   

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
