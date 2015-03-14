using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Util.Lock
{
    //Thanks to Scott Hanselman and Stephen Toubs
    //http://www.hanselman.com/blog/ComparingTwoTechniquesInNETAsynchronousCoordinationPrimitives.aspx

    public sealed class XAsyncLock
    {
        private readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(1, 1);
        private readonly Task<IDisposable> m_releaser;

        public XAsyncLock()
        {
            m_releaser = Task.FromResult((IDisposable)new Releaser(this));
        }

        public Task<IDisposable> LockAsync()
        {
            var wait = m_semaphore.WaitAsync();
            return wait.IsCompleted ?
                        m_releaser :
                        wait.ContinueWith((_, state) => (IDisposable)state,
                            m_releaser.Result, CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        private sealed class Releaser : IDisposable
        {
            private readonly XAsyncLock m_toRelease;
            internal Releaser(XAsyncLock toRelease) { m_toRelease = toRelease; }
            public void Dispose() { m_toRelease.m_semaphore.Release(); }
        }
    }

    public static class XNamedLock
    {
        private static readonly Dictionary<string, XAsyncLock> Locks = new Dictionary<string, XAsyncLock>();

        private static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();


        public static XAsyncLock Get(string name)
        {
            _lock.EnterUpgradeableReadLock();

            try
            {
                if (Locks.ContainsKey(name))
                {
                    return Locks[name];
                }

                _lock.EnterWriteLock();

                try
                {
                    var newLock = new XAsyncLock();
                    Locks.Add(name, newLock);

                    return newLock;

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

        public static async Task Clear()
        {
            Locks.Clear();
        }
    }
}
