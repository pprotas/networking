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
        /// <summary>
        /// The port at which the desired server is reachable.
        /// </summary>
        private int Port { get; }

        /// <summary>
        /// The URL at which the desired server is reachable.
        /// </summary>
        public Uri Url => new Uri($"http://localhost:{Port}/");

        /// <summary>
        /// A simple HTTP client which requests data from an external server.
        /// </summary>
        /// <param name="port"></param>
        public Client(int port)
        {
            Port = port;
        }

        /// <summary>
        /// Performs a GET request at the desired server.
        /// </summary>
        /// <param name="cancellationToken">The CancellationToken which gets used to cancel the request when possible.</param>
        /// <returns></returns>
        public async Task<string> GetAsync(CancellationToken cancellationToken)
        {
            using HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(Url, cancellationToken).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return content;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Connection to {Url} cancelled.");
                return null;
            }
        }
    }
}
