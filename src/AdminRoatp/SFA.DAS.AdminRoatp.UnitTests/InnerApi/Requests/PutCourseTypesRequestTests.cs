using FluentAssertions;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class PutCourseTypesRequestTests
{
    [Test, MoqAutoData]
    public void WhenBuildingRequest_ThenPropertiesAreMappedFromModel(
        AddCourseTypesModel model,
        int ukprn)
    {
        var request = new PutCourseTypesRequest(ukprn, model);

        request.Ukprn.Should().Be(ukprn);
        request.Data.Should().BeSameAs(model);
    }

    [Test, MoqAutoData]
    public void WhenBuildingRequest_ThenPutUrlIsSetCorrectly(
        AddCourseTypesModel model,
        int ukprn)
    {
        var request = new PutCourseTypesRequest(ukprn, model);

        request.PutUrl.Should().Be($"/providers/{ukprn}/course-types");
    }
}
