using SFA.DAS.EmployerFinance.InnerApi.Requests.Commitments;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingGetCommittedLearnersCostByAccountIdRequest
{
    [Test, MoqAutoData]
    public void Then_The_Request_Url_Is_Correctly_Formed(long accountId, int pageNumber, int pageItemCount, int? transferSenderId)
    {
        var request = new GetCommittedLearnersCostByAccountIdRequest(accountId, pageNumber, pageItemCount, transferSenderId);
        request.GetUrl.Should().Be($"api/apprenticeships?accountId={accountId}&transferSenderId={transferSenderId}&pageNumber={pageNumber}&pageItemCount={pageItemCount}");
    }

    [Test, MoqAutoData]
    public void Then_The_Request_Url_Is_Correctly_Formed_When_TransferSenderId_Is_Null(long accountId, int pageNumber, int pageItemCount)
    {
        var request = new GetCommittedLearnersCostByAccountIdRequest(accountId, pageNumber, pageItemCount, null);
        request.GetUrl.Should().Be($"api/apprenticeships?accountId={accountId}&pageNumber={pageNumber}&pageItemCount={pageItemCount}");
    }
}