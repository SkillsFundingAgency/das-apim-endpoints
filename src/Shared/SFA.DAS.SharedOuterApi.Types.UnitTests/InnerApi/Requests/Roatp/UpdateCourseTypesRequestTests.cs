using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests.Roatp;

internal class UpdateCourseTypesRequestTests
{
    [Test, AutoData]
    public void UpdateCourseTypesRequest_ValidatesPutUrl(
        int ukprn,
        UpdateCourseTypesModel model)
    {
        var expectedUrl = $"/organisations/{ukprn}/course-types";

        UpdateCourseTypesRequest apiRequest = new(ukprn, model);

        apiRequest.PutUrl.Should().Be(expectedUrl);
    }

    [Test, AutoData]
    public void UpdateCourseTypesRequest_ValidatesDataModell(
        int ukprn,
        UpdateCourseTypesModel model)
    {
        UpdateCourseTypesRequest apiRequest = new(ukprn, model);

        apiRequest.data.Should().BeEquivalentTo(model);
    }
}
