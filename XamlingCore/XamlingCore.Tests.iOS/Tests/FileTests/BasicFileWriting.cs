using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using XamlingCore.iOS.Implementations;

namespace XamlingCore.Tests.iOS.Tests.FileTests
{
    [TestFixture]
    public class BasicFileWriting
    {
        [Test]
        public void TestWriteStream()
        {
            var b = new byte[100];

            var ls = new LocalStorage();


             var msr = new ManualResetEvent(false);

            Task.Run(async () =>
            {
                using (var ms = new MemoryStream(b))
                {
                    var fn = Path.Combine("TestWrites", Guid.NewGuid().ToString());
                    await ls.SaveStream(fn, ms);

                    Assert.IsTrue(await ls.FileExists(fn));
                    using (var result = await ls.LoadStream(fn))
                    {
                        Assert.IsNotNull(result);
                        Assert.AreEqual(result.Length, 100);
                    }
                }

                msr.Set();
            });

            var msrResult = msr.WaitOne(5000);
            Assert.IsTrue(msrResult, "MSR not set, means assertion failed in task");

          
            
        }
    }
}
