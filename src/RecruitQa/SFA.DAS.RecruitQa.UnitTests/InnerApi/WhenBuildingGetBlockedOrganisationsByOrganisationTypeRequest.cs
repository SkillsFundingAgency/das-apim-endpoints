using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingGetBlockedOrganisationsByOrganisationTypeRequest
{
    [Test, MoqAutoData]
    public void Then_The_GetBlockedOrganisationsByOrganisationTypeRequest_Url_Is_Correct(string organisationType)
    {
        //Act
        var actual = new GetBlockedOrganisationsByOrganisationTypeRequest(organisationType);

        //Assert
        actual.GetAllUrl.Should().Be($"api/blockedorganisations?organisationType={organisationType}");
    }
}