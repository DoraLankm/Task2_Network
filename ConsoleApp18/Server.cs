using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    internal class Server
    {
        public static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        public static CancellationToken token = cancelTokenSource.Token;


        public static async Task AcceptMs()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
            using UdpClient ucl = new UdpClient(12345);
            Console.WriteLine("Сервер ожидает сообщения от клиента");


                while (!token.IsCancellationRequested)
                {
                    if (ucl.Available > 0)
                    {
                        byte[] buffer = ucl.Receive(ref iPEndPoint);
                        string str = Encoding.UTF8.GetString(buffer);
                        if (str == "Exit")
                        {
                            cancelTokenSource.Cancel();
                            continue;
                        }
                        Message? message = Message.FromJson(str);
                        Console.WriteLine(message.ToString());

                        Message ms = new Message("Server", "Сообщение отправлено");
                        string json = ms.ToJson();
                        byte[] bytes = Encoding.UTF8.GetBytes(json);
                        await ucl.SendAsync(bytes, iPEndPoint);
                    }
                    else
                    {
                        await Task.Delay(100);
                    }
                }

        }
    }
}