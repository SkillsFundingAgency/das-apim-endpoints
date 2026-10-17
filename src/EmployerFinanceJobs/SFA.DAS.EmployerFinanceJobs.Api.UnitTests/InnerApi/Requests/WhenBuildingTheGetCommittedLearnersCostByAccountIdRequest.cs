using FluentAssertions;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingTheGetCommittedLearnersCostByAccountIdRequest
{
    [Test, MoqAutoData]
    public void WhenBuildingTheRequest_ThenTheCorrectUrlIsGenerated(
        long accountId,
        int pageNumber,
        int pageItemCount,
        long transferSenderId)
    {
        // Act
        var request = new GetCommittedLearnersCostByAccountIdRequest(
            accountId,
            pageNumber,
            pageItemCount,
            transferSenderId);

        // Assert
        request.GetUrl.Should().Be(
            $"api/apprenticeships?accountId={accountId}&transferSenderId={transferSenderId}&pageNumber={pageNumber}&pageItemCount={pageItemCount}");
    }

    [Test, MoqAutoData]
    public void WhenBuildingTheRequestWithNoAccountId_ThenTheCorrectUrlIsGenerated(
        int pageNumber,
        int pageItemCount,
        long transferSenderId)
    {
        // Act
        var request = new GetCommittedLearnersCostByAccountIdRequest(
            null,
            pageNumber,
            pageItemCount,
            transferSenderId);

        // Assert
        request.GetUrl.Should().Be(
            $"api/apprenticeships?transferSenderId={transferSenderId}&pageNumber={pageNumber}&pageItemCount={pageItemCount}");
    }

    [Test, MoqAutoData]
    public void WhenBuildingTheRequestWithNoTransferSenderId_ThenTheCorrectUrlIsGenerated(
        long accountId,
        int pageNumber,
        int pageItemCount)
    {
        // Act
        var request = new GetCommittedLearnersCostByAccountIdRequest(
            accountId,
            pageNumber,
            pageItemCount,
            null);

        // Assert
        request.GetUrl.Should().Be(
            $"api/apprenticeships?accountId={accountId}&pageNumber={pageNumber}&pageItemCount={pageItemCount}");
    }
}
