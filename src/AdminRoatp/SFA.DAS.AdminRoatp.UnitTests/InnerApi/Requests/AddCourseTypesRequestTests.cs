using FluentAssertions;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class AddCourseTypesRequestTests
{
    [Test, MoqAutoData]
    public void WhenBuildingRequest_ThenPropertiesAreMappedFromModel(
        AddCourseTypesModel model,
        int ukprn)
    {
        // Act
        var request = new AddCourseTypesRequest(ukprn, model);

        // Assert
        request.Ukprn.Should().Be(ukprn);
        request.Data.Should().BeSameAs(model);
    }

    [Test, MoqAutoData]
    public void WhenBuildingRequest_ThenPostUrlIsSetCorrectly(
        AddCourseTypesModel model,
        int ukprn)
    {
        // Act
        var request = new AddCourseTypesRequest(ukprn, model);

        // Assert
        request.PostUrl.Should().Be($"/providers/{ukprn}/course-types");
    }
}
