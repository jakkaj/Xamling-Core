using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Data.DataExtensions;
using XamlingCore.Tests.iOS.Base;

namespace XamlingCore.Tests.iOS.Tests.EntityTests
{
    [TestFixture]
    public class EntityBucketTests : TestBase
    {
        public class TestBuckets
        {
            public const string Bucket1 = "Bucket1";
            public const string Bucket2 = "Bucket2";
        }

        [Test]
        public void TestInstanceCorrect()
        {
            var entityManager = Container.Resolve<IEntityManager<EntityTests.TestEntity>>();

            var testItems = _testData();

            var msr = new ManualResetEvent(false);

            Task.Run(async () =>
            {
                foreach (var i in testItems)
                {
                    await i.Set();
                    if (i.IsSomething)
                    {
                        await i.AddToBucket(TestBuckets.Bucket1);
                    }
                    else
                    {
                        await i.AddToBucket(TestBuckets.Bucket2);
                    }
                }

                foreach (var i in testItems)
                {
                    if (i.IsSomething)
                    {
                        Assert.IsTrue(await i.IsInBucket(TestBuckets.Bucket1));
                        Assert.IsFalse(await i.IsInBucket(TestBuckets.Bucket2));
                    }
                    else
                    {
                        Assert.IsTrue(await i.IsInBucket(TestBuckets.Bucket2));
                        Assert.IsFalse(await i.IsInBucket(TestBuckets.Bucket1));
                    }
                }

                var allInBucket1 = await entityManager.AllInBucket(TestBuckets.Bucket1);
                var allInBucket2 = await entityManager.AllInBucket(TestBuckets.Bucket2);

                foreach (var i in testItems)
                {
                    if (i.IsSomething)
                    {
                        Assert.IsTrue(allInBucket1.Contains(i));
                        Assert.IsFalse(allInBucket2.Contains(i));
                    }
                    else
                    {
                        Assert.IsFalse(allInBucket1.Contains(i));
                        Assert.IsTrue(allInBucket2.Contains(i));
                    }
                }

                foreach (var i in testItems)
                {
                    if (i == testItems.First() || i == testItems.Last())
                    {
                        await i.Delete();
                        var allInBucket11 = await entityManager.AllInBucket(TestBuckets.Bucket1);
                        var allInBucket21 = await entityManager.AllInBucket(TestBuckets.Bucket2);

                        Assert.IsFalse(allInBucket11.Contains(i));
                        Assert.IsFalse(allInBucket21.Contains(i));
                    }
                }

                testItems.Remove(testItems.FirstOrDefault());
                testItems.Remove(testItems.LastOrDefault());

                foreach (var i in testItems)
                {
                    if (i == testItems.First() || i == testItems.Last())
                    {
                        Assert.IsTrue(await i.IsInBucket(TestBuckets.Bucket1) || await i.IsInBucket(TestBuckets.Bucket2));

                        await i.RemoveFromBucket(TestBuckets.Bucket1);
                        await i.RemoveFromBucket(TestBuckets.Bucket2);

                        Assert.IsFalse(await i.IsInBucket(TestBuckets.Bucket1) && await i.IsInBucket(TestBuckets.Bucket2));
                    }
                }

                msr.Set();
            });

            var msrResult = msr.WaitOne(1115000);
            Assert.IsTrue(msrResult, "MSR not set, means assertion failed in task");
        }

        private List<EntityTests.TestEntity> _testData()
        {
            var l = new List<EntityTests.TestEntity>();

            for (var i = 0; i < 15; i++)
            {
                l.Add(new EntityTests.TestEntity {Id = Guid.NewGuid(), IsSomething = i%2 == 0});
            }

            return l;

        }
    }
}

