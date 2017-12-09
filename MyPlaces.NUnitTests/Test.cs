using NUnit.Framework;
using System;
namespace MyPlaces.NUnitTests
{
    [TestFixture()]
    public class Test
    {
        [Test()]
        public void DistanceCalculatorTest()
        {
            Double longitudeHSR = 47.223198;
            Double latitudeHSR = 8.817658;

            Double longitudeChurBHPlatz = 46.8528119;
            Double latitudeChurBHPlatz = 9.5272742;

            Double distanceInM = Math.Round(MyPlaces.Standard.Data.DistanceCalculator.GetDistanceInMetres(latitudeHSR, longitudeHSR, latitudeChurBHPlatz, longitudeChurBHPlatz));
            Double distanceInKm = distanceInM / 1000;

            Assert.AreEqual(distanceInKm, 88.761);
        }
    }
}
