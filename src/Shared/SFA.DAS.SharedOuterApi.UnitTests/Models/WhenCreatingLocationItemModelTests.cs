using FluentAssertions;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.UnitTests.Models;

public sealed class WhenCreatingLocationItemModelTests
{
    [Test]
    public void When_GeoPoints_Are_Valid_Then_Latitude_An_Longitude_Should_Return_Correct_Values()
    {
        var geoPoint = new double[] { 52.123456, -1.987654 };
        var locationItem = new LocationItem("Location", geoPoint, "UK");

        locationItem.Latitude.Should().Be((decimal?)52.123456m);
        locationItem.Longitude.Should().Be((decimal?)(-1.987654m));
    }

    [Test]
    public void When_GeoPoints_Is_Null_Then_Latitude_An_Longitude_Should_Return_Null()
    {
        var locationItem = new LocationItem("Test Location", null, "UK");

        Assert.Multiple(() =>
        {
            Assert.That(locationItem.Latitude, Is.Null);
            Assert.That(locationItem.Longitude, Is.Null);
        });
    }

    [Test]
    public void When_GeoPoints_Is_Empty_Then_Latitude_An_Longitude_Should_Return_Null()
    {
        var locationItem = new LocationItem("Location", [], "UK");

        Assert.Multiple(() =>
        {
            Assert.That(locationItem.Latitude, Is.Null);
            Assert.That(locationItem.Longitude, Is.Null);
        });
    }

    [Test]
    public void When_GeoPoints_Has_One_Item_Then_Latitude_Should_Only_Be_Returned()
    {
        var geoPoint = new double[] { 40.712776 };
        var locationItem = new LocationItem("Test Location", geoPoint, "USA");


        Assert.Multiple(() =>
        {
            Assert.That(locationItem.Latitude, Is.EqualTo((decimal?)40.712776m));
            Assert.That(locationItem.Longitude, Is.Null);
        });
    }
}
