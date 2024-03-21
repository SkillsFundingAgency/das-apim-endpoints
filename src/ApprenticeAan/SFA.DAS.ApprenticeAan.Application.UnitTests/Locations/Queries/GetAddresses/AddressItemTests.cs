using AutoFixture.NUnit3;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetAddresses;


namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Locations.Queries.GetAddresses;

public class AddressItemTests
{
    [Test]
    [AutoData]
    public void Operator_ConvertsFrom_GetAddressesListItem(GetAddressesListItem source)
    {
        AddressItem sut = source;

        Assert.Multiple(() =>
        {
            Assert.That(sut.Uprn, Is.EqualTo(source.Uprn));
            Assert.That(sut.OrganisationName, Is.EqualTo(source.Organisation));
            Assert.That(sut.Town, Is.EqualTo(source.PostTown));
            Assert.That(sut.County, Is.EqualTo(source.County));
            Assert.That(sut.Postcode, Is.EqualTo(source.Postcode));
            Assert.That(sut.AddressLine1, Is.EqualTo(source.AddressLine1));
            Assert.That(sut.AddressLine2, Is.EqualTo(source.AddressLine2));
            Assert.That(sut.Longitude, Is.EqualTo(source.Longitude));
            Assert.That(sut.Latitude, Is.EqualTo(source.Latitude));
        });
    }
}
