using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Tests.BigWindows.Mapper
{
    [TestClass]
    public class SimpleMapperTests
    {
        [TestMethod]
        public void TestMapperShallow()
        {
            var c = new TestClass
            {
                Name = "Jordan",
                Others = new List<OtherClass>
                {
                    new OtherClass
                    {
                        Name = "Other 1"
                    },
                    new OtherClass
                    {
                        Name = "Other 2"
                    }
                }
            };

            var c2 = new TestClass();


            c.CopyProperties(c2);

            Assert.IsTrue(c2.Name == c.Name);
            Assert.IsTrue(object.ReferenceEquals(c2.Others, c.Others));
        }
    }



    public class TestClass : IEntity
    {
        public string Name { get; set; }
        public List<OtherClass> Others { get; set; }

        public Guid Id { get; set; }
    }

    public class OtherClass
    {
        public string Name { get; set; }
    }
}
