using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using XamlingCore.Droid.Implementations;

namespace XamlingCore.Droid.Tests.FileTests
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

            var msrResult = msr.WaitOne(25000);
            Assert.IsTrue(msrResult, "MSR not set, means assertion failed in task");



        }

        [Test]
        public void TestWriteStreamLots()
        {

            var ls = new LocalStorage();

           
            
            var msr = new ManualResetEvent(false);

            string test = "";
            for (var i = 0; i < 20; i++)
            {
                test += "this here is more text" + i;
            }

            Task.Run(async () =>
            {
                for (var b = 0; b < 50; b++)
                {


                    var g = Guid.NewGuid().ToString();
                    for (var i = 0; i < 10; i++)
                    {

                        var fn = Path.Combine("TestWrites", g);
                        //var fn = "TestWrites";
                        // test += i;

                        await ls.SaveString(fn, test);

                        Assert.IsTrue(await ls.FileExists(fn));
                        var loaded = await ls.LoadString(fn);

                        Assert.IsNotNull(loaded);
                        Assert.AreEqual(loaded, test);
                       // Debug.WriteLine(i);
                    }
                }

                msr.Set();
            });


            Task.Run(async () =>
            {
                for (var b = 0; b < 50; b++)
                {


                    var g = Guid.NewGuid().ToString();
                    for (var i = 0; i < 10; i++)
                    {

                        var fn = Path.Combine("TestWrites", g);
                        //var fn = "TestWrites";
                        // test += i;

                        await ls.SaveString(fn, test);

                        Assert.IsTrue(await ls.FileExists(fn));
                        var loaded = await ls.LoadString(fn);

                        Assert.IsNotNull(loaded);
                        Assert.AreEqual(loaded, test);
                        // Debug.WriteLine(i);
                    }
                }

                //msr.Set();
            });

            Task.Run(async () =>
            {
                for (var b = 0; b < 50; b++)
                {


                    var g = Guid.NewGuid().ToString();
                    for (var i = 0; i < 10; i++)
                    {

                        var fn = Path.Combine("TestWrites", g);
                        //var fn = "TestWrites";
                        // test += i;

                        await ls.SaveString(fn, test);

                        Assert.IsTrue(await ls.FileExists(fn));
                        var loaded = await ls.LoadString(fn);

                        Assert.IsNotNull(loaded);
                        Assert.AreEqual(loaded, test);
                        // Debug.WriteLine(i);
                    }
                }

                //msr.Set();
            });


            var msrResult = msr.WaitOne(25000);
            Assert.IsTrue(msrResult, "MSR not set, means assertion failed in task");



        }
    }
}
