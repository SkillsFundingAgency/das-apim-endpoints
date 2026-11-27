using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingGetVacancyReviewsByVacancyReferenceRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Constructed_Correctly(long vacancyReference, string status)
    {
        var actual = new GetVacancyReviewsByVacancyReferenceRequest(vacancyReference, "",false);

        actual.GetUrl.Should().Be($"api/vacancies/{vacancyReference}/Reviews?includeNoStatus=False");
    }

    [Test, AutoData]
    public void Then_The_Url_Includes_ManualOutcome_When_Provided(long vacancyReference, string status, List<string> manualOutcome)
    {
        var actual = new GetVacancyReviewsByVacancyReferenceRequest(vacancyReference, status, false, manualOutcome);

        actual.GetUrl.Should().Be($"api/vacancies/{vacancyReference}/Reviews?includeNoStatus=False&status={status}&manualOutcome={string.Join("&manualOutcome=", manualOutcome)}");
    }
}
