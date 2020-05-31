using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hammer.Core.Callsigns;
using Hammer.Core.Helpers;

namespace HammerTests
{
    [TestClass]
    public class TestPrefixes
    {
        [TestMethod]
        public void TestTryGetRegionUS()
        {
            const string Expected = "us";
            string actual;
            Prefixes.TryGetRegion("W", out actual);
            Assert.AreEqual(Expected, actual);
        }

        [TestMethod]
        public void TestTryGetRegionOOWS()
        {
            const string Expected = "oows";
            string actual;
            Prefixes.TryGetRegion("S0", out actual);
            Assert.AreEqual(Expected, actual);
        }
    }

    [TestClass]
    public class TestRegionalIndicators
    {        
        [TestMethod]
        public void TestTryGetIndicatorPair()
        {
            const string Expected = "🇺🇸";
            string actual;
            RegionalIndicators.TryGetIndicatorPair("us", out actual);
            Assert.AreEqual(Expected, actual);
        }
    }
}
