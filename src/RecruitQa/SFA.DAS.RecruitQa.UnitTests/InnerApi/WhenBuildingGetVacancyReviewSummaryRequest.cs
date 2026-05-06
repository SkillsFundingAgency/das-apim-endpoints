using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingGetVacancyReviewSummaryRequest
{
    [Test]
    public void Then_The_Url_Is_Constructed_Correctly()
    {
        var actual = new GetVacancyReviewSummaryRequest();

        actual.GetUrl.Should().Be("api/VacancyReviews/summary");
    }
}
