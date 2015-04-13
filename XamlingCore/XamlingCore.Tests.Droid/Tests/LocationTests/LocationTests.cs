using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using XamlingCore.Tests.Droid.Base;
using XamlingCore.Droid.Implementations;
using XamlingCore.Portable.Model.Location;

namespace XamlingCore.Tests.Droid.Tests.LocationTests
{
    [TestFixture]
    public class LocationTests : TestBase
    {
        LocationTrackingSensor s;

        [TestFixtureSetUp]
        public void Init()
        {
            s = new LocationTrackingSensor();
            s.StartTracking();
        }

        [Test]
        public void TestLocationTracker()
        {
            Task<XLocation> l = s.GetQuickLocation();
            l.Wait();

            if (l.Result != null)
            {
                Console.WriteLine("Location: " + l.Result.Latitude + " | " + l.Result.Longitude);
            }

            Assert.IsNotNull(l.Result);
        }

        [Test]
        public void TestLocationDistance()
        {
            int distanceMelbToSyd = 714; //k's.

            XLocation melbourne = new XLocation
            {
                Latitude = -37.8136000,
                Longitude = 144.9631000
            };
            XLocation sydney = new XLocation
            {
                Latitude = -33.8650000,
                Longitude = 151.2094000
            };

            double d = s.Distance(melbourne.Latitude, melbourne.Longitude, sydney);


            int i = (int)Math.Round(d);
            Console.WriteLine("Distance to sydney: " + i);

            //~714k

            Assert.AreEqual(i, distanceMelbToSyd);
        }

    }
}