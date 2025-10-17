using System;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.InnerApi;

public class WhenBuildingGetVacancyReviewByIdRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Built_Correctly(Guid id)
    {
        var actual = new GetVacancyReviewByIdRequest(id);
        
        actual.GetUrl.Should().Be($"api/vacancyreviews/{id}");
    }
}