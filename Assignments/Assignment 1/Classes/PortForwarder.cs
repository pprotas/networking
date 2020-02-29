using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Networking.Assignment1
{
    public class PortForwarder
    {
        private List<Client> clients;
        private readonly HttpListener listener;
        private readonly int port;
        public string Url => $"http://localhost:{port}/";
        private string pageData;
        public string PageData { get => pageData; set => pageData = value; }
        public PortForwarder(List<Client> clients, int port)
        {
            this.clients = clients;
            this.port = port;
            PageData = pageData;
            listener = new HttpListener();
            listener.Prefixes.Add(Url);
        }

        public async Task RunAsync()
        {
            while (true)
            {
                PageData = await GetFirstPageData();

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

        private async Task<string> GetFirstPageData()
        {
            List<string> pageDatas = new List<string>();
            List<Task> tasks = new List<Task>();

            foreach(Client client in clients)
            {
                tasks.Add(Task.Run(async () => await GetPageData(client)));
            }

            Task.WaitAny(tasks.ToArray());
            return pageDatas[0];

            async Task GetPageData(Client client)
            {
                pageDatas.Add(await client.RunAsync());
            }
        }


    }
}
