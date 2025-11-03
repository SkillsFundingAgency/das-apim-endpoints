using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetOrganisations;
public class GetOrganisationsQueryResultTests
{
    [Test, AutoData]

    public void GetOrganisationsQueryResult_MapsDataCorrectly(
        OrganisationResponse getOrganisationDetails)
    {
        // Arrange
        OrganisationSummary organisationSummary = getOrganisationDetails;
        GetOrganisationsQueryResult sut = new() { Organisations = [organisationSummary] };

        // Act
        var result = sut.Organisations.First();

        // Assert
        result.Ukprn.Should().Be(getOrganisationDetails.Ukprn);
        result.LegalName.Should().Be(getOrganisationDetails.LegalName);
    }
}