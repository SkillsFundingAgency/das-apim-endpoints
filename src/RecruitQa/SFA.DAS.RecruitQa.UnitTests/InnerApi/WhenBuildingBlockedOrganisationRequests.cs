using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingGetBlockedOrganisationByOrganisationIdRequest
{
    [Test, MoqAutoData]
    public void Then_The_GetBlockedOrganisationByOrganisationIdRequest_Url_Is_Correct(string organisationId)
    {
        //Act
        var actual = new GetBlockedOrganisationByOrganisationIdRequest(organisationId);

        //Assert
        actual.GetUrl.Should().Be($"api/blockedorganisations/byorganisationid/{organisationId}");
    }
}