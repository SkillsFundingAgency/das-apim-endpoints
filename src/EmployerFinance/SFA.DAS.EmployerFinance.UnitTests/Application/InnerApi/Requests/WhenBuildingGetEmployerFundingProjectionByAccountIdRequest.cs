using SFA.DAS.EmployerFinance.InnerApi.Requests;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingGetEmployerFundingProjectionByAccountIdRequest
{
    [Test, MoqAutoData]
    public void Then_The_Request_Url_Is_Correctly_Formed(long accountId)
    {
        var request = new GetEmployerFundingProjectionByAccountIdRequest(accountId);
        request.GetUrl.Should().Be($"api/employer/{accountId}/funding-projection");
    }
}