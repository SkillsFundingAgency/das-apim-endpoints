using FluentAssertions;
using SFA.DAS.AdminRoatp.Application.Commands.UpsertProviderAllowedCourse;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class UpsertProviderAllowedCourseRequestTests
{
    [Test, MoqAutoData]
    public void WhenBuildingRequest_ThenPropertiesAreMappedFromCommand(
        UpsertProviderAllowedCourseCommand command)
    {
        // Act
        var request = new UpsertProviderAllowedCourseRequest(command);

        // Assert
        request.Ukprn.Should().Be(command.Ukprn);
        request.LarsCode.Should().Be(command.LarsCode);
        request.Data.Should().BeSameAs(command);
    }

    [Test, MoqAutoData]
    public void WhenBuildingRequest_ThenPostUrlIsSetCorrectly(
        UpsertProviderAllowedCourseCommand command)
    {
        // Act
        var request = new UpsertProviderAllowedCourseRequest(command);

        // Assert
        request.PostUrl.Should().Be($"providers/{command.Ukprn}/allowed-courses/{command.LarsCode}");
    }
}
