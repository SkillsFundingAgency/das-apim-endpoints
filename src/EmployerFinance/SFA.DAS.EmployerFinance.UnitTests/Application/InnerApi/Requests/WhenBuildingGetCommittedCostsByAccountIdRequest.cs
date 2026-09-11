using SFA.DAS.EmployerFinance.InnerApi.Requests;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingGetCommittedCostsByAccountIdRequest
{
    [Test, MoqAutoData]
    public void Then_The_Request_Url_Is_Correctly_Formed(long accountId)
    {
        var request = new GetCommittedCostsByAccountIdRequest(accountId);
        request.GetUrl.Should().Be($"api/committed-costs/{accountId}");
    }
}