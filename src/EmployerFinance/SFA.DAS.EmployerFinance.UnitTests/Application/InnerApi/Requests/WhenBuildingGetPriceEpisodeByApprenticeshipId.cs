using SFA.DAS.EmployerFinance.InnerApi.Requests.Commitments;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingGetPriceEpisodeByApprenticeshipId
{
    [Test, MoqAutoData]
    public void Then_The_Request_Url_Is_Correctly_Formed(long apprenticeshipId)
    {
        var request = new GetPriceEpisodeByApprenticeshipIdRequest(apprenticeshipId);
        request.GetUrl.Should().Be($"api/apprenticeships/{apprenticeshipId}/price-episodes");
    }
}