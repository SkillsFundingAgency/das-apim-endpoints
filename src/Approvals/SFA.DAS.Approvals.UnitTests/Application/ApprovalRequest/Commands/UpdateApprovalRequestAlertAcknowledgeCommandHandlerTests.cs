using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Application.ApprovalRequest.Commands;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.ApprovalRequest.Commands;

public class UpdateApprovalRequestAlertAcknowledgeCommandHandlerTests
{
    private UpdateApprovalRequestAlertAcknowledgeRequest _request;
    private UpdateApprovalRequestAlertAcknowledgeCommand command;
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient;
    private UpdateApprovalRequestAlertAcknowledgeCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();
        _request = fixture.Create<UpdateApprovalRequestAlertAcknowledgeRequest>();
        command = fixture.Create<UpdateApprovalRequestAlertAcknowledgeCommand>();
        apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        _handler = new UpdateApprovalRequestAlertAcknowledgeCommandHandler(apiClient.Object);
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_A_Valid_Request()
    {
        apiClient.Setup(x => x.PutWithResponseCode<NullResponse>(It.Is<UpdateApprovalRequestAlertAcknowledgeRequest>
            (t => t.ApprenticeshipId == command.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.OK, string.Empty));

        await _handler.Handle(command, CancellationToken.None);
        apiClient.Verify(x => x.PutWithResponseCode<NullResponse>
        (It.Is<UpdateApprovalRequestAlertAcknowledgeRequest>(t => t.ApprenticeshipId == command.ApprenticeshipId)), Times.Once);
    }
}