using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Data.Extensions;
using XamlingCore.Portable.Model.Contract;
using XamlingCore.Tests.BigWindows.Base;

namespace XamlingCore.Tests.BigWindows.Entity
{
    [TestClass]
    public class EntityInstanceTests : TestBase
    {
        [TestMethod]
        public async Task EntitySaveAndLoad()
        {
            var p = new Person { Id = Guid.NewGuid() };

            var service = Resolve<IEntityManager<Person>>();

            var pSaved = await service.Set(p);

            //load the person again
            var pLoaded = await service.Get(p.Id);

            Assert.IsTrue(Object.ReferenceEquals(pSaved, pLoaded));

            var p2 = new Person
            {
                Id = p.Id,
                Age = 21,
                Name = "Billingsworth Canterburington"
            };

            p2 = await p2.Set();

            Assert.AreEqual(pSaved.Name, p2.Name);
            Assert.IsTrue(Object.ReferenceEquals(pSaved, p2));
        }

        [TestMethod]
        public async Task TestInstanceCorrect()
        {
            var entityManager = Resolve<IEntityManager<TestEntity>>();
            var entityManager2 = Resolve<IEntityManager<TestEntity>>();

            var testItems = _testData();

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
                Assert.IsTrue(ReferenceEquals(i2, i));

            }

            foreach (var i in testItems)
            {
                var i2 = await entityManager2.Get(i.Id);

                var newE = new TestEntity { Id = i.Id };

                newE.Name = "Knight";

                newE = await entityManager.Set(newE);

                Assert.IsTrue(i2.Name == "Knight");
                Assert.IsTrue(ReferenceEquals(i2, newE));
                Assert.IsTrue(ReferenceEquals(i, newE));
                Assert.IsTrue(ReferenceEquals(i, i2));
            }

            foreach (var i in testItems)
            {
                var iGot = await entityManager.Get(i.Id);
                Assert.IsTrue(ReferenceEquals(iGot, i));
                await entityManager.Delete(i);
                var iGot2 = await entityManager.Get(i.Id);
                Assert.IsNull(iGot2);
            }





        }

        List<TestEntity> _testData()
        {
            var l = new List<TestEntity>();

            for (var i = 0; i < 10; i++)
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

    public class Person : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }


}
