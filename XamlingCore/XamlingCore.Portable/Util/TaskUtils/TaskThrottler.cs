//With the guidance from teh various posts on this StackOverflow http://stackoverflow.com/questions/22492383/throttling-asynchronous-tasks
//Rolled in with some AsyncLock bits from http://www.hanselman.com/blog/ComparingTwoTechniquesInNETAsynchronousCoordinationPrimitives.aspx
//And a little of my own flavour


using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly string _name;
        private int _concurrentItems;

        private int _currentCount = 0;

        private SemaphoreSlim _semaphore;

        private readonly Task<IDisposable> m_releaser;

        protected TaskThrottler(string name, int concurrentItems = 10)
        {
            _name = name;
            _concurrentItems = concurrentItems;
            _semaphore = new SemaphoreSlim(concurrentItems);
            m_releaser = Task.FromResult((IDisposable)new Releaser(this));
        }

        public Task<IDisposable> LockAsync()
        {

            var wait = _semaphore.WaitAsync();

            if (!wait.IsCompleted)
            {
                Debug.WriteLine("TaskThrottler throttled " + _name + " at " + _concurrentItems);
            }

            return wait.IsCompleted ?
                        m_releaser :
                        wait.ContinueWith((_, state) => (IDisposable)state,
                            m_releaser.Result, CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }


        public Task<T> Throttle<T>(Func<Task<T>> sourceTask)
        {
            var t = new TaskCompletionSource<T>();

            Task.Run(async () =>
            {
                var wait = _semaphore.WaitAsync();

                if (!wait.IsCompleted)
                {
                    Debug.WriteLine("TaskThrottler throttled " + _name + " at " + _concurrentItems);
                }

                await wait;

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

        public async Task<bool> Throttle(Func<Task> sourceTask)
        {
            async Task<bool> func()
            {
                await sourceTask();
                return true;
            }

            return await Throttle(func);
        }

        private static readonly Dictionary<string, TaskThrottler> Throttles = new Dictionary<string, TaskThrottler>();


        public static TaskThrottler GetNetwork(int concurrentItems = 10)
        {
            return Get("DefaultNetworkThrottler", concurrentItems);
        }

        private static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public static TaskThrottler Get(string name, int? concurrentItems = null)
        {
            _lock.EnterUpgradeableReadLock();

            try
            {
                if (Throttles.ContainsKey(name))
                {
                    return Throttles[name];
                }

                _lock.EnterWriteLock();

                try
                {
                    var newThrottle = new TaskThrottler(name, concurrentItems ?? 10);

                    Throttles.Add(name, newThrottle);

                    Debug.WriteLine("Added name");

                    return newThrottle;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }

            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }

        }

        private sealed class Releaser : IDisposable
        {
            private readonly TaskThrottler m_toRelease;
            internal Releaser(TaskThrottler toRelease) { m_toRelease = toRelease; }
            public void Dispose() { m_toRelease._semaphore.Release(); }
        }

    }
}
