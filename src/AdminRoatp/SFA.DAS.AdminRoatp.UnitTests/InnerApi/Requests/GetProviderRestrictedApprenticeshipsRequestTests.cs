using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class GetProviderRestrictedApprenticeshipsRequestTests
{
    [Test, AutoData]
    public void WhenCreatingRequest_ThenSetsCorrectUrl(int ukprn)
    {
        // Arrange
        var sut = new GetProviderRestrictedApprenticeshipsRequest(ukprn);

        // Assert
        sut.GetUrl.Should().Be($"providers/{ukprn}/restricted-apprenticeships");
    }
}
