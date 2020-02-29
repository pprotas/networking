using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Networking.Assignment1
{
    class Program
    {
        public static Random random = new Random();

        static async Task Main(string[] args)
        {
            Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            string pageData =
            "<!DOCTYPE>" +
            "<html>" +
            "  <head>" +
            "    <title>WebServer</title>" +
            "  </head>" +
            $"  <body bgcolor=\"{ColorTranslator.ToHtml(randomColor)}\">" +
            "  </body>" +
            "</html>";

            Console.Write("Port: ");
            Server server = new Server(int.Parse(Console.ReadLine()), pageData);
            await server.RunAsync();
        }
    }
}
