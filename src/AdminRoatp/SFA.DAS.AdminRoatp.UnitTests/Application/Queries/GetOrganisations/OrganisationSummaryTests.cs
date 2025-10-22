using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetOrganisations;
public class OrganisationSummaryTests
{
    [Test, AutoData]
    public void OrganisationSummary_MapsOrganisationSummary(
        OrganisationModel getOrganisationDetails)
    {
        // Act
        OrganisationSummary sut = getOrganisationDetails;

        // Assert
        sut.Ukprn.Should().Be(getOrganisationDetails.Ukprn);
        sut.LegalName.Should().Be(getOrganisationDetails.LegalName);
    }
}