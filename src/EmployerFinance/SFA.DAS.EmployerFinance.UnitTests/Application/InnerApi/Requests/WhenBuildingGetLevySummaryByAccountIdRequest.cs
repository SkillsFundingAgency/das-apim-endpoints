using SFA.DAS.EmployerFinance.InnerApi.Requests;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingGetLevySummaryByAccountIdRequest
{
    [Test, MoqAutoData]
    public void Then_The_Request_Url_Is_Correctly_Formed(long accountId)
    {
        var request = new GetLevySummaryByAccountIdRequest(accountId);

        request.GetUrl.Should().Be($"api/levy-declarations/{accountId}/summary");
    }
}