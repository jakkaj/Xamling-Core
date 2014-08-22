using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Nito.AsyncEx;
using NUnit.Framework;
using XamlingCore.Portable.Contract.Cache;
using XamlingCore.Tests.iOS.Base;

namespace XamlingCore.Tests.iOS.Tests.CacheTests
{
    [TestFixture]
    public class CacheTests : TestBase
    {

        [Test]
        public void Check_Parallel_Test()
        {
            var cache = Container.Resolve<IEntityCache>();

            var msr = new ManualResetEvent(false);

            Task.Run(async () =>
            {
                var list = new List<string>();

                await cache.SetEntity("EmptyList", list);

                var a = cache.GetEntity<List<string>>("EmptyList", _serverGetListGuid, null, true, false);
                var b = cache.GetEntity<List<string>>("EmptyList", _serverGetListGuid, null, true, false);
                var cc = cache.GetEntity<List<string>>("EmptyList", _serverGetListGuid, null, true, false);
                var d = cache.GetEntity<List<string>>("EmptyList", _serverGetListGuid, null, true, false);
                var e = cache.GetEntity<List<string>>("EmptyList", _serverGetListGuid, null, true, false);
                var f = cache.GetEntity<List<string>>("EmptyList", _serverGetListGuid, null, true, false);

                var result = await Task.WhenAll(a, b, cc, d, e, f);

                var firsResult = result.FirstOrDefault();

                Assert.IsNotNull(firsResult);

                Assert.IsTrue(firsResult.Count != 0);

                var firstGuid = firsResult.FirstOrDefault();
                
                foreach (var item in result)
                {
                    Assert.AreEqual(item.FirstOrDefault(), firstGuid);
                }
                msr.Set();
            });

            var msrResult = msr.WaitOne(3000);
            Assert.IsTrue(msrResult, "MSR not set, means assertion failed in task");

        }
        async Task<List<string>> _serverGetListGuid()
        {
            await Task.Delay(200);
            return new List<string>()
            {
                Guid.NewGuid().ToString()
            };
        }
        [Test]
        public async Task Check_Zero_Lists()
        {
            var cache = Container.Resolve<IEntityCache>();

            var list = new List<string>();

            await cache.SetEntity("EmptyList", list);

            var listGet = await cache.GetEntity<List<string>>("EmptyList", _serverGetList, null, true, true);

            Assert.IsTrue(listGet.Count == 0);

            var listGetFromServer = await cache.GetEntity<List<string>>("EmptyList", _serverGetList, null, true, false);
            Assert.IsTrue(listGetFromServer.Count != 0);
            Assert.AreEqual(listGetFromServer.FirstOrDefault(), "Jordan");
        }

        async Task<List<string>> _serverGetList()
        {
            return new List<string>()
            {
                "Jordan"
            };
        }
        [Test]
        public async Task Cache_Get_AWAITER()
        {
            var cache = Container.Resolve<IEntityCache>();

            var cItem = await cache.GetEntity("AwaitedItem", () => GetItem("Awaited"), maxAge: TimeSpan.FromSeconds(5));

            Assert.IsNotNull(cItem);
            Assert.AreEqual(cItem.Field, "Awaited");

            var cItem2 = await cache.GetEntity<CacheTest2>("AwaitedItem", TimeSpan.FromSeconds(5));
            Assert.IsNotNull(cItem2);
            Assert.AreEqual(cItem2.Field, "Awaited");


            var cItem3 = await cache.GetEntity("AwaitedItem", () => GetItem("Awaited2"), maxAge: TimeSpan.FromSeconds(5));

            //the new item will not have done the call back, as it was cached
            Assert.IsNotNull(cItem3);
            Assert.AreEqual(cItem3.Field, "Awaited");

            await cache.Delete<CacheTest2>("AwaitedItem");
            var cItem4 = await cache.GetEntity("AwaitedItem", () => GetItem("Awaited3"), maxAge: TimeSpan.FromSeconds(5));
            Assert.IsNotNull(cItem4);
            Assert.AreEqual(cItem4.Field, "Awaited3");
        }

        private async Task<CacheTest2> GetItem(string fieldName)
        {
            return new CacheTest2 { Field = fieldName };
        }

        [Test]
        public async Task Cache_Get_Set()
        {
            var cache = Container.Resolve<IEntityCache>();

            var l1 = new List<CacheTest1>();
            var l2 = new List<CacheTest2>();

            for (var i = 0; i < 15; i++)
            {
                var c1 = new CacheTest1
                {
                    Field = "F" + i
                };

                l1.Add(c1);

                await cache.SetEntity(c1.Field, c1);

                var c2 = new CacheTest2
                {
                    Field = "F" + i
                };

                l2.Add(c2);

                await cache.SetEntity(c2.Field, c2);
            }

            await cache.SetEntity("List1", l1);
            await cache.SetEntity("List2", l2);

            var c1Out = await cache.GetEntity<List<CacheTest1>>("List1", TimeSpan.FromSeconds(5));
            var c2Out = await cache.GetEntity<List<CacheTest2>>("List2", TimeSpan.FromSeconds(5));

            Assert.IsNotNull(c1Out);
            Assert.IsNotNull(c2Out);

            Assert.AreEqual(c2Out.Count, 15);
            Assert.AreEqual(c1Out.Count, 15);

            await cache.Delete<List<CacheTest1>>("List1");

            var c1Out2 = await cache.GetEntity<List<CacheTest1>>("List1", TimeSpan.FromSeconds(5));
            var c2Out2 = await cache.GetEntity<List<CacheTest2>>("List2", TimeSpan.FromSeconds(5));

            var c1OutMissingBadName = await cache.GetEntity<List<CacheTest1>>("List3", TimeSpan.FromSeconds(5));

            Assert.IsNotNull(c2Out2);
            Assert.IsNull(c1Out2);
            Assert.IsNull(c1OutMissingBadName);

            var c1Out3 = await cache.GetEntity<List<CacheTest2>>("List1", TimeSpan.FromSeconds(5));
            var c2Out3 = await cache.GetEntity<List<CacheTest1>>("List2", TimeSpan.FromSeconds(5));

            Assert.IsNull(c2Out3);
            Assert.IsNull(c1Out3);

            var f1 = await cache.GetEntity<CacheTest1>("F1", TimeSpan.FromSeconds(5));
            var f2 = await cache.GetEntity<CacheTest2>("F1", TimeSpan.FromSeconds(5));

            Assert.IsNotNull(f1);
            Assert.IsNotNull(f2);


            await cache.Delete<CacheTest1>("F1");

            var f12 = await cache.GetEntity<CacheTest1>("F1", TimeSpan.FromSeconds(5));
            var f22 = await cache.GetEntity<CacheTest2>("F1", TimeSpan.FromSeconds(5));

            Assert.IsNull(f12);
            Assert.IsNotNull(f22);
        }

        [Test]
        public async Task Cache_Timeout_test()
        {
            var cache = Container.Resolve<IEntityCache>();

            var c2 = new CacheTest2
            {
                Field = "FJK"
            };

            await cache.SetEntity("FJK", c2);

            var beforeTimeout = await cache.GetEntity<CacheTest2>("FJK", TimeSpan.FromSeconds(1));

            await Task.Delay(2000);
            var afterTimeout = await cache.GetEntity<CacheTest2>("FJK", TimeSpan.FromSeconds(1));

            Assert.IsNotNull(beforeTimeout);
            Assert.IsNull(afterTimeout);
        }

    }

    public class CacheTest1
    {
        public string Field { get; set; }
    }

    public class CacheTest2
    {
        public string Field { get; set; }
    }
}