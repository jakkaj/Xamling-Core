//With the guidance from teh various posts on this StackOverflow
//http://stackoverflow.com/questions/22492383/throttling-asynchronous-tasks

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XamlingCore.Portable.Util.Lock;

namespace XamlingCore.Portable.Util.TaskUtils
{
    /// <summary>
    /// Throttle your asynchronous operations. 
    /// Remember - don't await the response when you're adding, group em up and await a List of tasks. 
    /// See the unit test TaskThrottleTests for examples
    /// </summary>
    public class TaskThrottler
    {
        private int _concurrentItems;

        private int _currentCount = 0;

        AsyncLock _throttleLock = new AsyncLock();
        AsyncLock _countLock = new AsyncLock();

        private SemaphoreSlim _semaphore;

        public TaskThrottler(int concurrentItems = 10)
        {
            _semaphore = new SemaphoreSlim(concurrentItems);
        }


        public Task<T> Throttle<T>(Func<Task<T>> sourceTask)
        {
            var t = new TaskCompletionSource<T>();

            Task.Run(async () =>
            {
                await _semaphore.WaitAsync();

                try
                {
                    var result = await sourceTask();
                    t.SetResult(result);
                }
                finally
                {
                    _semaphore.Release();
                }
            });

            return t.Task;
        }

        public Task<bool> Throttle(Func<Task> sourceTask)
        {
            var t = new TaskCompletionSource<bool>();

            Task.Run(async () =>
            {
                await _semaphore.WaitAsync();

                try
                {
                    await sourceTask();
                    t.SetResult(true);
                }
                finally
                {
                    _semaphore.Release();
                }
            });

            return t.Task;
        }

        private static readonly Dictionary<string, TaskThrottler> Throttles = new Dictionary<string, TaskThrottler>();

        static ManualResetEvent msr = new ManualResetEvent(true);

        public static TaskThrottler GetNetwork(int concurrentItems = 10)
        {
            return Get("DefaultNetworkThrottler", concurrentItems);
        }

        public static TaskThrottler Get(string name, int? concurrentItems = null)
        {
            if (Throttles.ContainsKey(name))
            {
                return Throttles[name];
            }

            msr.WaitOne();

            if (Throttles.ContainsKey(name))
            {
                return Throttles[name];
            }

            var newThrottle = new TaskThrottler(concurrentItems ?? 10);
            Throttles.Add(name, newThrottle);

            msr.Set();

            return newThrottle;
        }

    }
}
