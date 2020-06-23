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
        /// <summary>
        /// The port at which this server is reachable.
        /// </summary>
        private int Port { get; set; }

        /// <summary>
        /// The URL at which this server is reachable.
        /// </summary>
        public Uri Url => new Uri($"http://localhost:{Port}/");

        /// <summary>
        /// The HttpListener which listens for incoming requests.
        /// </summary>
        protected HttpListener Listener { get; set; }

        /// <summary>
        /// The data which gets hosted on the server.
        /// </summary>
        public string PageData { get; set; }

        /// <summary>
        /// A simple HTTP server which hosts an HTML file.
        /// </summary>
        /// <param name="port">The port at which this server is reachable.</param>
        /// <param name="pageData">The data which gets hosted on the server.</param>
        public Server(int port, string pageData)
        {
            Port = port;
            PageData = pageData;
            Listener = new HttpListener();
            Listener.Prefixes.Add(Url.ToString());
        }

        /// <summary>
        /// Listens to incoming requests after performing an initialization.
        /// </summary>
        /// <returns></returns>
        public async Task RunAsync()
        {
            await InitializeRunAsync().ConfigureAwait(false);

            while (true)
            {
                await LoopAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Runs any desired tasks required before LoopAsync().
        /// </summary>
        /// <returns></returns>
        protected virtual async Task InitializeRunAsync()
        {
            await Task.Run(Listener.Start).ConfigureAwait(false);
        }

        /// <summary>
        /// The tasks to perform in an infinite loop during RunAsync().
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoopAsync()
        {
            await PerformListenAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Listen to a single request and send PageData to the client.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task PerformListenAsync()
        {
            HttpListenerContext context = await Listener.GetContextAsync().ConfigureAwait(false);
            HttpListenerResponse response = context.Response;

            byte[] data = Encoding.UTF8.GetBytes(PageData);

            response.ContentType = "text/html";
            response.ContentEncoding = Encoding.UTF8;
            response.ContentLength64 = data.LongLength;

            await response.OutputStream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
            response.Close();
        }
    }
}
