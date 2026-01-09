using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingGetVacancyReviewsByAccountLegalEntityRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Constructed_Correctly(long accountLegalEntityId)
    {
        var actual = new GetVacancyReviewsByAccountLegalEntityRequest(accountLegalEntityId);

        actual.GetUrl.Should().Be($"accounts/{accountLegalEntityId}/vacancyreviews");
    }
}