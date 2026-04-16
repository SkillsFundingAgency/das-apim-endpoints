using FluentAssertions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetOrganisationTypes;
public class GetOrganisationTypesRequestTests
{
    [Test]
    public void Test()
    {
        // Arrange
        GetOrganisationTypesRequest sut = new();
        var expectedUrl = "organisation-types";

        // Act
        var url = sut.GetUrl;

        // Assert
        url.Should().Be(expectedUrl);
    }
}