using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Networking.Assignment1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<Client> clients = new List<Client>();

            bool promptPort = true;
            while (promptPort)
            {
                Console.WriteLine("Enter localhost port to connect to (1-10000, 0 = stop): ");

                int port = -1;
                try
                {
                    port = int.Parse(Console.ReadLine());

                    if (port == 0) promptPort = false;

                    else if (port < 1 || port > 10000) throw new FormatException();

                    else clients.Add(new Client(port));
                }
                catch (FormatException)
                {
                    Console.Write("Please enter a valid number between 0 and 10000");
                }
            }

            PortForwarder portForwarder = new PortForwarder(clients, 8080);
            await portForwarder.RunAsync();
        }
    }
}
