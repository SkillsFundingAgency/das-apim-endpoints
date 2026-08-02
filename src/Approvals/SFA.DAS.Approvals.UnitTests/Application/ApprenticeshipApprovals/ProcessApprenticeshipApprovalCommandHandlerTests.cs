using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Application.ApprenticeshipApprovals.Commands.ProcessApprenticeshipApproval;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.ApprenticeshipApprovals;

[TestFixture]
public class ProcessApprenticeshipApprovalCommandHandlerTests
{
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
    private ProcessApprenticeshipApprovalCommandHandler _handler;
    private ProcessApprenticeshipApprovalRequest _request;

    [SetUp]
    public void SetUp()
    {
        _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        _apiClient.Setup(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<ProcessApprenticeshipApprovalRequest>(),
                false))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.OK, string.Empty));

        _handler = new ProcessApprenticeshipApprovalCommandHandler(
            _apiClient.Object);
    }

    [Test]
    public async Task Handle_WhenApplyingApproval_PostEndpointIsCalledCorrectly()
    {
        var fixture = new Fixture();

        var command = fixture.Create<ProcessApprenticeshipApprovalCommand>();

        await _handler.Handle(command, CancellationToken.None);

        _apiClient.Verify(x => x.PostWithResponseCode<NullResponse>(
            It.Is<ProcessApprenticeshipApprovalRequest>(r =>
                r.PostUrl == $"api/apprenticeships/{command.ApprenticeshipId}/approvals/{command.ApprovalRequestId}" &&
                ((ProcessApprenticeshipApprovalRequest.Body)r.Data).UserInfo == command.UserInfo &&
                ((ProcessApprenticeshipApprovalRequest.Body)r.Data).ApplyChanges == command.ApplyChanges),
            false), Times.Once);
    }
}
