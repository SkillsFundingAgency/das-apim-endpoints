using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingGetLevyLastSubmissionDateRequest
{
    [Test, MoqAutoData]
    public void Then_The_Request_Url_Is_Correctly_Formed(long accountId)
    {
        var request = new GetLevyLastSubmissionDateRequest(accountId);
        request.GetUrl.Should().Be($"api/levy-declarations/{accountId}/last-submission-date");
    }
}