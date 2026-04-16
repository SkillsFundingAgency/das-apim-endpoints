using FluentAssertions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetOrganisations;
public class GetOrganisationsRequestTests
{
    [Test, MoqAutoData]
    public void GetOrganisationsRequest_ValidateGetUrl(
        GetOrganisationsRequest sut)
    {
        // Arrange
        var expectedUrl = "organisations";

        // Act & Assert
        sut.GetUrl.Should().Be(expectedUrl);
    }
}