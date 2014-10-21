using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Model.Contract;
using XamlingCore.Tests.iOS.Base;

namespace XamlingCore.Tests.iOS.Tests.EntityTests
{
    [TestFixture]
    public class EntityTests : TestBase
    {
        [Test]
        public void TestInstanceCorrect()
        {
            var entityManager = Container.Resolve<IEntityManager<TestEntity>>();
            var entityManager2 = Container.Resolve<IEntityManager<TestEntity>>();

            var testItems = _testData();

            entityManager.Set(testItems);

            var msr = new ManualResetEvent(false);

            Task.Run(async () =>
            {
                foreach (var i in testItems)
                {
                    var iGot = await entityManager.Get(i.Id);
                    Assert.IsTrue(ReferenceEquals(iGot, i));
                    var iGot2 = await entityManager2.Get(i.Id);
                    Assert.IsTrue(ReferenceEquals(iGot, iGot2));
                    Assert.IsTrue(ReferenceEquals(i, iGot2));
                }
                msr.Set();
            });

            var msrResult = msr.WaitOne(20000);
            Assert.IsTrue(msrResult, "MSR not set, means assertion failed in task");



        }

        List<TestEntity> _testData()
        {
            var l = new List<TestEntity>();

            for (var i = 0; i < 200; i++)
            {
                l.Add(new TestEntity { Id = Guid.NewGuid() });
            }

            return l;

        }

        public class TestEntity : IEntity
        {
            public Guid Id { get; set; }
        }
    }
}