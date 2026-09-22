using SFA.DAS.EmployerFinance.InnerApi.Requests;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingGetEmployerFundingProjectionByAccountIdRequest
{
    [Test, MoqAutoData]
    public void Then_The_Request_Url_Is_Correctly_Formed(long accountId, int months)
    {
        var request = new GetEmployerFundingProjectionByAccountIdRequest(accountId, months);
        request.GetUrl.Should().Be($"api/employer/{accountId}/funding-projection?months={months}");
    }
}