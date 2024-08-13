using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    internal class Client
    {
        private static IPEndPoint iPEndPoint;
        private static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        private static CancellationToken token = cancelTokenSource.Token;

        public static async Task StartAsync(int port, string name)
        {
           
            iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            UdpClient ucl = new UdpClient();

            Task receiveTask = ReceiveMessageAsync(ucl);
            Task sendTask = SendMsgAsync(name, ucl);

            await Task.WhenAny(receiveTask, sendTask);
            cancelTokenSource.Cancel(); // Отмена всех задач при завершении

            await Task.WhenAll(receiveTask, sendTask); // Дождаться завершения всех задач
        }


        private static async Task SendMsgAsync(string nik, UdpClient ucl)
        {
           CancellationToken token = cancelTokenSource.Token;

            while (!token.IsCancellationRequested)
            {
                Console.WriteLine("To write the name of user");
                string user = Console.ReadLine();
                if (String.IsNullOrEmpty(user))
                {
                    Console.WriteLine("The message is null");
                    continue;
                }
                Console.WriteLine("To write a message");
                string text = Console.ReadLine();
                if (text.ToLower().Equals("exit"))
                {
                    await ucl.SendAsync(Encoding.UTF8.GetBytes(text), iPEndPoint);
                    cancelTokenSource.Cancel();
                }
                else
                {
                    Message message = new Message(nik, text);
                    message.ToName = user;

                    Creator creator = new CreateJSON();
                    Format JSON = creator.FactoryMethod();

                    string json = JSON.ToJsonOrXML(message);
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    await ucl.SendAsync(bytes, iPEndPoint);
                }

                
            }
        }

        private static async Task ReceiveMessageAsync(UdpClient ucl) //ожидание сообщений
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (ucl.Available > 0)
                    {
                        var result = await ucl.ReceiveAsync();
                        string str = Encoding.UTF8.GetString(result.Buffer);
                        Creator creator = new CreateJSON();
                        Format JSON = creator.FactoryMethod();
                        Message? ms = JSON.FromJsonOrXML(str);
                        if (ms != null)
                        {
                            Console.WriteLine(ms.ToString());
                        }
                    }

                    else
                    {
                        await Task.Delay(100);
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
        }
    }
}