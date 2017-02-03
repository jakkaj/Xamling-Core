using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XamlingCore.Portable.Util.TaskUtils;

namespace XamlingCore.Tests.NET.ThrottleTests
{
    /// <summary>
    /// okay so it's a crap test, but you can see that it's working from the debug outputs... was in a bit of a rush!
    /// </summary>
    [TestClass]
    public class TaskThrottleTests
    {

        [TestMethod]
        public async Task TestAsyncThrottlingUsingDisposeMethod()
        {
            //this is a very poor method for loops, good for throttling across many threads.
            for (var i = 0; i < 500; i++)
            {

                var i2 = i;

                Task.Run(async () =>
                {
                    using (var l = await TaskThrottler.Get("Test", 2).LockAsync())
                    {
                        Debug.WriteLine("Added: {0}", i2);
                        await _doSomeWork();
                    }
                });
            }


            await Task.Delay(50000);

        }

        [TestMethod]
        public async Task TestAsyncThrottlingReturns()
        {
            List<Task<SomeEntity>> tasks = new List<Task<SomeEntity>>();

            for (var i = 0; i < 500; i++)
            {
                var t = TaskThrottler.Get("Test", 50).Throttle(_getAnEntity);
                tasks.Add(t);
                Debug.WriteLine("Added: {0}", i);
            }

            await Task.WhenAll(tasks);
        }

        [TestMethod]
        public async Task TestAsyncThrottlingNoReturnsLarger()
        {
            var throttler = TaskThrottler.Get("Test", 100);
            List<Task> tasks = new List<Task>();



            for (var i = 0; i < 50; i++)
            {
                 tasks.Add(throttler.Throttle(_doSomeWork));
               
                Debug.WriteLine("Added: {0}", i);
            }

            await Task.WhenAll(tasks);

            Assert.IsTrue(thiscount == 50);
        }

        [TestMethod]
        public async Task TestAsyncThrottlingNoReturnsSmaller()
        {
            var throttler = TaskThrottler.Get("Test", 10);
            List<Task> tasks = new List<Task>();



            for (var i = 0; i < 50; i++)
            {
                tasks.Add(throttler.Throttle(_doSomeWork));

                Debug.WriteLine("Added: {0}", i);
            }

            await Task.WhenAll(tasks);

            Assert.IsTrue(thiscount == 50);
        }

        [TestMethod]
        public async Task TestAsyncNoThrottlingNoReturns()
        {
            var throttler = TaskThrottler.Get("Test", 500);
            List<Task> tasks = new List<Task>();
            for (var i = 0; i < 5000; i++)
            {
                tasks.Add(_doSomeWork());

                Debug.WriteLine("Added: {0}", i);
            }

            await Task.WhenAll(tasks);
        }


        private int thiscount = 0;

        async Task _doSomeWork()
        {
            
            await Task.Delay(500);
            thiscount += 1;
            Debug.WriteLine("Did some work");
        }
        private static int count;
        async Task<SomeEntity> _getAnEntity()
        {
            var t = new SomeEntity()
            {
                Count = ++count
            };

            await Task.Delay(1000);

            Debug.WriteLine("Actual: {0}", t.Count);

            return t;
        }


        public class SomeEntity
        {
            public int Count { get; set; }
        }
    }
}
