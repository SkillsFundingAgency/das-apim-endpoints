using System;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.InnerApi;

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

public class WhenBuildingPutUserRequest
{
    [Test, AutoData]
    public void Then_The_Url_And_Date_Are_Constructed_And_Sent(UserDto data, Guid id)
    {
        var actual = new PutUserRequest(id, data);
        
        actual.PutUrl.Should().Be($"api/user/{id}");
        actual.Data.Should().BeEquivalentTo(data);
    }
}