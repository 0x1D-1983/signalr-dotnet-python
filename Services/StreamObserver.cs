using System;
using System.Threading.Tasks;

namespace SignalRFilterPlay.Services
{
    /// <summary>
    /// Observer implementation for streaming data.
    /// </summary>
    /// <typeparam name="T">The type of data being observed.</typeparam>
    public class StreamObserver<T> : IObserver<T>
    {
        private readonly Func<T, Task> _onNext;
        private readonly TaskCompletionSource _completionSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamObserver{T}"/> class.
        /// </summary>
        /// <param name="onNext">The action to execute when new data arrives.</param>
        /// <param name="completionSource">Task completion source for handling completion.</param>
        public StreamObserver(Func<T, Task> onNext, TaskCompletionSource completionSource)
        {
            _onNext = onNext;
            _completionSource = completionSource;
        }

        /// <summary>
        /// Called when the stream completes.
        /// </summary>
        public void OnCompleted()
        {
            _completionSource.TrySetResult();
        }

        /// <summary>
        /// Called when an error occurs in the stream.
        /// </summary>
        /// <param name="error">The error that occurred.</param>
        public void OnError(Exception error)
        {
            _completionSource.TrySetException(error);
        }

        /// <summary>
        /// Called when new data arrives in the stream.
        /// </summary>
        /// <param name="value">The new data value.</param>
        public void OnNext(T value)
        {
            try
            {
                _onNext(value).Wait();
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }
    }
}
