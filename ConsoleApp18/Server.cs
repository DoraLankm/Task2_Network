using System;
using System.ComponentModel.Design;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    internal class Server
    {
        static Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();

        static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        static CancellationToken token = cancelTokenSource.Token;


        public static async Task AcceptMsAsync()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
            using UdpClient ucl = new UdpClient(12345);
            Console.WriteLine("Waiting a message from client");


            while (!token.IsCancellationRequested)
            {
                if (ucl.Available > 0)
                {
                    byte[] buffer = ucl.Receive(ref iPEndPoint);
                    string str = Encoding.UTF8.GetString(buffer);
                    if (str.ToLower().Equals("exit"))
                    {
                        cancelTokenSource.Cancel();
                        continue;
                    }
                    Creator creator = new CreateJSON();
                    Format JSON = creator.FactoryMethod();

                    Message? message = JSON.FromJsonOrXML(str);
                   
                    Message answer_message = new Message();
                    answer_message.FromName = "Server";
                    answer_message.ToName = message.FromName;
                    if (message.ToName.Equals("Server"))
                    {
                        Console.WriteLine(message.ToString());
                        if (message.Text.ToLower().Equals("register"))
                        {
                            if (clients.TryAdd(message.FromName, iPEndPoint))
                            {
                                answer_message = new Message("Server", $"Successful registred {message.FromName}");
                            }
                        }

                        else if (message.Text.ToLower().Equals("delete"))
                        {
                            clients.Remove(message.FromName);
                            answer_message = new Message("Server", $"Delete user {message.FromName}");
                        }

                        else if (message.Text.ToLower().Equals("list"))
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach(var client in clients)
                            {
                                sb.Append(client.Key + Environment.NewLine);
                            }
                            answer_message = new Message("Server", $"List of users \n {sb.ToString()}");

                        }

                    }

                    else if (message.ToName.ToLower().Equals("all"))
                    {
                        Console.WriteLine($"{message} and send every user");

                        

                        foreach (var client in clients)
                        {

                            message.FromName = client.Key;
                            string json1 = JSON.ToJsonOrXML(message);
                            byte[] bytes1 = Encoding.UTF8.GetBytes(json1);
                            await ucl.SendAsync(bytes1, client.Value);
                            answer_message = new Message("Server", $"Sended to every users");
                        }

                    }

                    else if (clients.TryGetValue(message.ToName,out IPEndPoint? ToEndPoint))
                    {
                        

                        string json1 = JSON.ToJsonOrXML(message);
                        byte[] bytes1 = Encoding.UTF8.GetBytes(json1);
                        await ucl.SendAsync(bytes1, ToEndPoint);
                        answer_message = new Message("Server", $"Sended message to {message.ToName}");
                    }

                    else
                    {
                        answer_message = new Message("Server", $"User {message.ToName} not found ");
                    }

                    if (answer_message != null)
                    {
                        

                        string json = JSON.ToJsonOrXML(answer_message);
                        byte[] bytes = Encoding.UTF8.GetBytes(json);
                        await ucl.SendAsync(bytes, iPEndPoint);
                    }
                    
                }
                else
                {
                    await Task.Delay(100);
                }
            }

        }
    }
}