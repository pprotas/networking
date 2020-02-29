using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Assignment1
{
    public class Server
    {
        private readonly HttpListener listener;
        private readonly int port;
        public string Url => $"http://localhost:{port}/";

        private string pageData;
        public string PageData { get => pageData; set => pageData = value; }

        public Server(int port, string pageData)
        {
            this.port = port;
            PageData = pageData;
            listener = new HttpListener();
            listener.Prefixes.Add(Url);
        }

        public async Task RunAsync()
        {
            while (true)
            {
                listener.Start();

                HttpListenerContext context = await listener.GetContextAsync();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                byte[] data = Encoding.UTF8.GetBytes(PageData);

                response.ContentType = "text/html";
                response.ContentEncoding = Encoding.UTF8;
                response.ContentLength64 = data.LongLength;

                await response.OutputStream.WriteAsync(data, 0, data.Length);
                response.Close();
            }
        }
    }
}
