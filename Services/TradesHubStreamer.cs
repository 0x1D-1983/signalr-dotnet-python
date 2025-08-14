using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using SignalRFilterPlay.Data;
using SignalRFilterPlay.Hubs;
using SignalRFilterPlay.Services;

namespace SignalRFilterPlay.Services
{
    /// <summary>
    /// Streams trades to the TradesHub.
    /// </summary>
    public class TradesHubStreamer : BackgroundService
    {
        private readonly IHubContext<TradesHub> _hubContext;
        private readonly ObservableStream<EpexTrade> _observableStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="TradesHubStreamer"/> class.
        /// </summary>
        /// <param name="hubContext">The hub context.</param>
        /// <param name="observableStream">The observable stream.</param>
        public TradesHubStreamer(IHubContext<TradesHub> hubContext, ObservableStream<EpexTrade> observableStream)
        {
            _hubContext = hubContext;
            _observableStream = observableStream;
        }

        /// <summary>
        /// Executes the streamer.
        /// </summary>
        /// <param name="stoppingToken">The stopping token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var completionSource = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            var observer = new StreamObserver<EpexTrade>(async trade =>
            {
                Console.WriteLine($"Streaming trade: {JsonSerializer.Serialize(trade)}");

                var filters = TradesHub.GetFilters();
                foreach (var (connectionId, filter) in filters)
                {
                    if (trade.WhereAreaMatch(filter.ParentMarketArea))
                    {
                        await _hubContext.Clients.Client(connectionId).SendAsync("Trade", trade, stoppingToken);
                    }
                }

                

                // await _hubContext.Clients.All.SendAsync("Trade", trade, stoppingToken);

            }, completionSource);

            _observableStream.Subscribe(observer);

            return Task.CompletedTask;
        }
    }
}