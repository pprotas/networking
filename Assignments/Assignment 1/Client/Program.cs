using Networking.Assignment1.Resources;
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

            Console.WriteLine(Resource.PortsPrompt);
            bool askPortsPrompt = true;
            while (askPortsPrompt)
            {

                int port;
                try
                {
                    port = int.Parse(Console.ReadLine());

                    if (port == 0) askPortsPrompt = false;

                    else if (port < 1 || port > 10000) throw new FormatException();

                    else clients.Add(new Client(port));
                }
                catch (FormatException e)
                {
                    Console.WriteLine(Resource.InvalidPortPrompt);
                    Console.WriteLine(e);
                }
            }

            await new PortForwarder(8080, clients.ToArray()).RunAsync().ConfigureAwait(true);
        }
    }
}
