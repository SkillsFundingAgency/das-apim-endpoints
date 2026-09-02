using SFA.DAS.EmployerFinance.InnerApi.Requests;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingGetLevySummaryByHashedAccountIdRequest
{
    [Test, MoqAutoData]
    public void Then_The_Request_Url_Is_Correctly_Formed(string hashedAccountId)
    {
        var request = new GetLevySummaryByHashedAccountIdRequest(hashedAccountId);

        request.GetUrl.Should().Be($"api/accounts/{hashedAccountId}/levy/summary");
    }
}