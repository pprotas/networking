using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Networking.Assignment1
{
    public class Client
    {
        public static Random random = new Random();

        static CancellationTokenSource tokenSource = new CancellationTokenSource();
        CancellationToken token = tokenSource.Token;

        private readonly HttpClient client;
        private readonly int port;
        public string Url => $"http://localhost:{port}/";

        public Client(int port)
        {
            this.port = port;
            client = new HttpClient();
        }

        public async Task<string> RunAsync()
        {
            //if (token.IsCancellationRequested)
            //    return null;
            //System.Threading.Thread.Sleep(3000);//random.Next(1000));

            HttpResponseMessage response = await client.GetAsync(Url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            //tokenSource.Cancel();
            return content;
        }
    }
}
