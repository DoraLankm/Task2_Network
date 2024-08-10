using ConsoleApp18;
using System;

class Program
{
    
    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            await Server.AcceptMs();
        }
        else
        {
            await Client.SendMsg(args[0]);
           
        }
    }
}