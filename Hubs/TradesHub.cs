using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using SignalRFilterPlay.Data;
using SignalRFilterPlay.Hubs;

namespace SignalRFilterPlay.Hubs
{
    /// <summary>
    /// SignalR hub for trading data.
    /// </summary>
    // [Authorize]
    public class TradesHub : Hub
    {
        // Maps ConnectionId to filter
        private static readonly ConcurrentDictionary<string, SignalRClientFilter> _filters = new();

        /// <summary>
        /// Subscribes to the hub.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task Subscribe(SignalRClientFilter filter)
        {
            _filters.AddOrUpdate(Context.ConnectionId, filter, (_, _) => filter);

            await Clients.Caller.SendAsync("Subscribed", new { ok = true, connectionId = Context.ConnectionId, filter = filter});
        }

        /// <summary>
        /// Unsubscribes from the hub.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _filters.TryRemove(Context.ConnectionId, out _);
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Gets the client filters.
        /// </summary>
        /// <returns>A dictionary of connection IDs and filters.</returns>
        public static IReadOnlyDictionary<string, SignalRClientFilter> GetFilters() => _filters;
    }
}