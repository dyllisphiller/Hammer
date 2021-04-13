using Hammer.Core.Callsigns;
using Hammer.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HammerTests
{
    [TestClass]
    public class TestPrefixes
    {
        [TestMethod]
        public void TestTryGetRegionUS()
        {
            const string Expected = "us";
            Issuers.TryGetRegion("W", out string actual);
            Assert.AreEqual(Expected, actual);
        }

        [TestMethod]
        public void TestTryGetRegionOOWS()
        {
            const string Expected = "oows";
            Issuers.TryGetRegion("S0", out string actual);
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
            RegionalIndicators.TryGetIndicatorPair("us", out string actual);
            Assert.AreEqual(Expected, actual);
        }
    }
}
