using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingGetVacancyReviewsCountByAccountLegalEntityRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Constructed_Correctly(long accountLegalEntityId, string status, string manualOutcome, string employerNameOption)
    {
        var actual = new GetVacancyReviewsCountByAccountLegalEntityRequest(accountLegalEntityId, status, manualOutcome, employerNameOption);

        actual.GetUrl.Should().Be($"api/accounts/{accountLegalEntityId}/vacancyreviews/count?status={status}&manualOutcome={manualOutcome}&employerNameOption={employerNameOption}");
    }
}
