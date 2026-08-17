using FluentAssertions;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class GetProviderAllowedCoursesRequestTests
{
    [Test, MoqAutoData]
    public void WhenRequestIsCreated_ThenGetUrlIsCorrect(
        int ukprn,
        CourseType courseType)
    {
        // Arrange
        var sut = new GetProviderAllowedCoursesRequest(ukprn, courseType);

        // Act
        var result = sut.GetUrl;

        // Assert
        result.Should().Be($"providers/{ukprn}/allowed-courses?courseType={courseType}");
    }
}
