using ConsoleApp18;
using System;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Server.Start();
        }
        else
        {
            new Thread(() =>
            {
                Client.SendMsg(args[0]);
            }).Start();
        }
    }
}