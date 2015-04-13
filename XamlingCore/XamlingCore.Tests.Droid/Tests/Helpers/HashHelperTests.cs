using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using Autofac;
using NUnit.Framework;
using XamlingCore.iOS.Implementations.Helpers;
using XamlingCore.Tests.Droid.Base;

namespace XamlingCore.Tests.Droid.Tests.Helpers
{
    [TestFixture]
    public class HashHelperTests :TestBase
    {
        [Test]
        public void TestHashesMatch()
        {
            var hasher = Container.Resolve<IHashHelper>();

            var s1 = Encoding.UTF8.GetBytes("This here is Tommy");
            var s2 = Encoding.UTF8.GetBytes("This here is Tommy");
            var s3 = Encoding.UTF8.GetBytes("This here is not Tommy");

            var h1 = hasher.Hash(s1);
            var h2 = hasher.Hash(s2);
            var h3 = hasher.Hash(s3);

            Assert.IsTrue(h1 == h2);
            Assert.IsFalse(h1 == h3);
        }
    }
}

