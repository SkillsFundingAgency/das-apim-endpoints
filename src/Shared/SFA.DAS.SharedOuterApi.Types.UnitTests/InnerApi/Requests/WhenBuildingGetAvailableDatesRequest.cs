using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Reservations;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetAvailableDatesRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed_When_No_CourseId(long accountLegalEntityId)
    {
        var actual = new GetAvailableDatesRequest(accountLegalEntityId);

        actual.GetUrl.Should().Be($"api/rules/available-dates/{accountLegalEntityId}");
    }

    [Test, AutoData]
    public void Then_The_Url_Includes_CourseId_Query_When_CourseId_Provided(long accountLegalEntityId, string courseId)
    {
        var actual = new GetAvailableDatesRequest(accountLegalEntityId, courseId);

        actual.GetUrl.Should().Be($"api/rules/available-dates/{accountLegalEntityId}?courseId={System.Uri.EscapeDataString(courseId)}");
    }
}