using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EducationalOrganisations;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingIdentifiableOrganisationTypesRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build()
    {
        var actual = new IdentifiableOrganisationTypesRequest();

        var expected = "api/EducationalOrganisations/IdentifiableOrganisationTypes";

        actual.GetUrl.Should().Be(expected);
    }
}