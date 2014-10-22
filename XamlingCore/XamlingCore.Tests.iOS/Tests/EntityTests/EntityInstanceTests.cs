using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
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



            var msr = new ManualResetEvent(false);

            Task.Run(async () =>
            {
                await entityManager.Set(testItems);
                foreach (var i in testItems)
                {
                    var iGot = await entityManager.Get(i.Id);
                    Assert.IsTrue(ReferenceEquals(iGot, i));
                    var iGot2 = await entityManager2.Get(i.Id);
                    Assert.IsTrue(ReferenceEquals(iGot, iGot2));
                    Assert.IsTrue(ReferenceEquals(i, iGot2));
                }

                foreach (var i in testItems)
                {
                    i.Name = "Jordan";

                    await entityManager.Set(i);

                    var i2 = await entityManager2.Get(i.Id);

                    Assert.IsTrue(i2.Name == i.Name);
                    Assert.True(ReferenceEquals(i2, i));

                }

                foreach (var i in testItems)
                {
                    var newE = new TestEntity { Id = i.Id };

                    newE.Name = "Knight";

                    newE = await entityManager.Set(newE);

                    var i2 = await entityManager2.Get(i.Id);

                    Assert.IsTrue(i2.Name == newE.Name);
                    Assert.True(ReferenceEquals(i2, newE));
                    Assert.True(ReferenceEquals(i, newE));
                    Assert.True(ReferenceEquals(i, i2));
                }

                foreach (var i in testItems)
                {
                    var iGot = await entityManager.Get(i.Id);
                    Assert.IsTrue(ReferenceEquals(iGot, i));
                    await entityManager.Delete(i);
                    var iGot2 = await entityManager.Get(i.Id);
                    Assert.IsNull(iGot2);
                }

                msr.Set();
            });

            var msrResult = msr.WaitOne(5000);
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

            public string Name { get; set; }

            public bool IsSomething { get; set; }
        }

        public class TestEntity2 : IEntity
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public bool IsSomething { get; set; }
        }
    }
}