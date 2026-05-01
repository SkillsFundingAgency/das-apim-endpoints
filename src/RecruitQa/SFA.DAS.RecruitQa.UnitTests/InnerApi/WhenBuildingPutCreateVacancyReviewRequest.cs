using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingPutCreateVacancyReviewRequest
{
    [Test, AutoData]
    public void Then_the_Url_And_Data_Are_Constructed_And_Sent(VacancyReviewDto data, Guid id)
    {
        var actual = new PutCreateVacancyReviewRequest(id, data);

        actual.PutUrl.Should().Be($"api/vacancyreviews/{id}");
        actual.Data.Should().BeEquivalentTo(data);
    }
}