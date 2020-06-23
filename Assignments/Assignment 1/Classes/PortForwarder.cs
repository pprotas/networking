using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Networking.Assignment1
{
    public class PortForwarder : Server
    {
        /// <summary>
        /// An array of clients that source the data from the chosen servers.
        /// </summary>
        private Client[] Clients { get; set; }

        /// <summary>
        /// The CancellationToken which gets used to cancel GET requests when possible.
        /// </summary>
        private CancellationTokenSource CancellationTokenSource { get; set; }

        /// <summary>
        /// A simple HTTP server that forwards data from the chosen servers.
        /// </summary>
        /// <param name="port">The port at which this server is reachable.</param>
        /// <param name="clients">Used to retrieve data from the desired servers.</param>
        public PortForwarder(int port, params Client[] clients) : base(port, null)
        {
            Clients = clients;
        }

        /// <summary>
        /// The tasks to perform in an infinite loop during RunAsync().
        /// </summary>
        /// <returns></returns>
        protected override async Task LoopAsync()
        {
            PageData = await GetFirstPageDataAsync().ConfigureAwait(true);
            await base.LoopAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Returns page data from the first completed GET request.
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetFirstPageDataAsync()
        {
            CancellationTokenSource = new CancellationTokenSource();

            IEnumerable<Task<string>> firstPageDataTasksQuery =
                from client in Clients select client.GetAsync(CancellationTokenSource.Token);
            Task<string>[] firstPageDataTasks = firstPageDataTasksQuery.ToArray();
            Task<string> firstPageData = await Task.WhenAny(firstPageDataTasks).ConfigureAwait(false);

            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();

            return await firstPageData.ConfigureAwait(false);
        }
    }
}
