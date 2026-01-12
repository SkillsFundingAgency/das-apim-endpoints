using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingGetVacancyReviewsByVacancyReferenceRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Constructed_Correctly(long vacancyReference, string status)
    {
        var actual = new GetVacancyReviewsByVacancyReferenceRequest(vacancyReference, status);

        actual.GetUrl.Should().Be($"{vacancyReference}/VacancyReviews?status={status}");
    }
}
