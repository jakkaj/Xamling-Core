using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }

    public class Person : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}
