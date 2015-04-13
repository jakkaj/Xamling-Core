using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using NUnit.Framework.Internal;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Data.Extensions;
using XamlingCore.Tests.Droid.Base;

namespace XamlingCore.Tests.Droid.Tests.EntityTests
{
    [TestFixture]
    public class EntityBucketTests : TestBase
    {
        public class TestBuckets
        {
            public const string Bucket1 = "Bucket1";
            public const string Bucket2 = "Bucket2";
            public const string Bucket3 = "Bucket1";
            public const string Bucket4 = "Bucket4";
        }

        [Test]
        public void TestBucketTypesNoMSR()
        {
            var entityManager = Container.Resolve<IEntityManager<EntityTests.TestEntity>>();

            var testItems = _testData();
            var testItems2 = _testData2();

            foreach (var i in testItems)
            {
                var i2 = i;
                Task.Run(() => i2.Set());
                Task.Run(() => i2.AddToBucket(TestBuckets.Bucket1));
            }

            foreach (var i in testItems2)
            {
                var i2 = i;
                Task.Run(() => i2.Set());
                Task.Run(() => i2.AddToBucket(TestBuckets.Bucket1));
            }

            //ensure that test item2 are in their own bucket one but not the other bucket 1

            var allInBucketTask = Task.Run(() => entityManager.AllInBucket(TestBuckets.Bucket1));
            var allInBucket = allInBucketTask.Result;

            foreach (var item in testItems2)
            {
                var i2 = item;
                var tTask = Task.Run(() => i2.IsInBucket(TestBuckets.Bucket1));
                var tResult = tTask.Result;
                Assert.IsTrue(tResult);
                Assert.IsNull(allInBucket.FirstOrDefault(_ => _.Id == item.Id));
            }
        }


        [Test]
        public void TestBucketTypes()
        {
            var entityManager = Container.Resolve<IEntityManager<EntityTests.TestEntity>>();

            var testItems = _testData();
            var testItems2 = _testData2();

            var msr = new ManualResetEvent(false);

            Task.Run(async () =>
            {
                foreach (var i in testItems)
                {
                    await i.Set();
                    await i.AddToBucket(TestBuckets.Bucket1);
                }

                foreach (var i in testItems2)
                {
                    await i.Set();
                    await i.AddToBucket(TestBuckets.Bucket1);
                }

                //ensure that test item2 are in their own bucket one but not the other bucket 1

                var allInBucket = await entityManager.AllInBucket(TestBuckets.Bucket1);

                foreach (var item in testItems2)
                {
                    Assert.IsTrue(await item.IsInBucket(TestBuckets.Bucket1));
                    Assert.IsNull(allInBucket.FirstOrDefault(_ => _.Id == item.Id));
                }

                msr.Set();
            });

            var msrResult = msr.WaitOne(5000);
            Assert.IsTrue(msrResult, "MSR not set, means assertion failed in task");
        }

        [Test]
        public void TestBucketMove()
        {
           
            var entityManager = Container.Resolve<IEntityManager<EntityTests.TestEntity>>();
           

            var testItems = _testData();

            var msr = new ManualResetEvent(false);

            Task.Run(async () =>
            {
                foreach (var i in testItems)
                {
                    await i.AddToBucket(TestBuckets.Bucket1);
                }

                foreach (var i in testItems)
                {
                    Assert.IsTrue(await i.IsInBucket(TestBuckets.Bucket1));
                    Assert.IsFalse(await i.IsInBucket(TestBuckets.Bucket4));

                    await i.MoveToBucket(TestBuckets.Bucket4);
                    Assert.IsFalse(await i.IsInBucket(TestBuckets.Bucket1));
                    Assert.IsTrue(await i.IsInBucket(TestBuckets.Bucket4));
                }

                msr.Set();
            });

            var msrResult = msr.WaitOne(5000);
            Assert.IsTrue(msrResult, "MSR not set, means assertion failed in task");
        }

        [Test]
        public void TestBucketNames()
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

            for (var i = 0; i < 100; i++)
            {
                l.Add(new EntityTests.TestEntity { Id = Guid.NewGuid(), IsSomething = i % 2 == 0 });
            }

            return l;

        }

        private List<EntityTests.TestEntity2> _testData2()
        {
            var l = new List<EntityTests.TestEntity2>();

            for (var i = 0; i < 15; i++)
            {
                l.Add(new EntityTests.TestEntity2 { Id = Guid.NewGuid(), IsSomething = i % 2 == 0 });
            }

            return l;

        }
    }
}

