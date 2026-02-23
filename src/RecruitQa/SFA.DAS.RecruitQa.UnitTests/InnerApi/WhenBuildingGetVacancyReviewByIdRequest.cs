using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingGetVacancyReviewByIdRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Built_Correctly(Guid id)
    {
        var actual = new GetVacancyReviewByIdRequest(id);
        
        actual.GetUrl.Should().Be($"api/vacancyreviews/{id}");
    }
}