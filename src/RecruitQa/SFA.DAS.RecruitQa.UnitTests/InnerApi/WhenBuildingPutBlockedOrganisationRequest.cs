using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingPutBlockedOrganisationRequest
{
    [Test, MoqAutoData]
    public void Then_The_PutBlockedOrganisationRequest_Url_And_Data_Are_Correct(Guid id, BlockedOrganisationDto data)
    {
        //Act
        var actual = new PutBlockedOrganisationRequest(id, data);

        //Assert
        actual.PutUrl.Should().Be($"/api/blockedorganisations/{id}");
        actual.Data.Should().Be(data);
    }
}