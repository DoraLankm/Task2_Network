using ConsoleApp18;
using System;

class Program
{
    
    static async Task Main(string[] args)
    {

        if (args.Length == 0)
        {
            await Server.AcceptMsAsync();
        }
        else
        {
            if (args.Length == 2)
            {
                int port = 0;
                if (Int32.TryParse(args[0], out port))
                {
                    await Client.StartAsync(port, args[1]);
                }
            }


        }
    }
}