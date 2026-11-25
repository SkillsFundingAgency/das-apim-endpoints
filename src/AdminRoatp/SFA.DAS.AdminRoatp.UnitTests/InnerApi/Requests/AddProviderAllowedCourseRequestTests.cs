using FluentAssertions;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class AddProviderAllowedCourseRequestTests
{
    [Test, MoqAutoData]
    public void WhenBuildingRequest_ThenPropertiesAreMappedFromCommand(
        AddProviderAllowedCourseModel model,
        int ukprn,
        string larsCode)
    {
        // Act
        var request = new AddProviderAllowedCourseRequest(ukprn, larsCode, model);

        // Assert
        request.Ukprn.Should().Be(ukprn);
        request.LarsCode.Should().Be(larsCode);
        request.Data.Should().BeSameAs(model);
    }

    [Test, MoqAutoData]
    public void WhenBuildingRequest_ThenPostUrlIsSetCorrectly(
        AddProviderAllowedCourseModel model,
        int ukprn,
        string larsCode)
    {
        // Act
        var request = new AddProviderAllowedCourseRequest(ukprn, larsCode, model);

        // Assert
        request.PostUrl.Should().Be($"providers/{ukprn}/allowed-courses/{larsCode}");
    }
}
