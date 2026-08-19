using FluentAssertions;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class RestrictProviderRequestTests
{
    [Test, MoqAutoData]
    public void WhenBuildingRequest_ThenPropertiesAreMappedFromCommand(
        RestrictProviderModel model,
        int ukprn,
        CourseType courseType)
    {
        // Act
        var request = new RestrictProviderRequest(ukprn, courseType, model);

        // Assert
        request.Ukprn.Should().Be(ukprn);
        request.CourseType.Should().Be(courseType);
        request.Data.Should().BeSameAs(model);
    }

    [Test, MoqAutoData]
    public void WhenBuildingRequest_ThenPostUrlIsSetCorrectly(
        RestrictProviderModel model,
        int ukprn,
        CourseType courseType)
    {
        // Act
        var request = new RestrictProviderRequest(ukprn, courseType, model);

        // Assert
        request.PostUrl.Should().Be($"providers/{ukprn}/course-types/{courseType}/restrict");
    }
}
