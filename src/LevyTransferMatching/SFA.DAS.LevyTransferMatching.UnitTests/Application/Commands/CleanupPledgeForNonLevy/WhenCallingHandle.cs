using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.LevyTransferMatching.Application.Commands.CleanupPledgeForNonLevy;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Constants;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.LevyTransferMatching;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CleanupPledgeForNonLevy;

[TestFixture]
public class WhenCallingHandle
{
    private readonly Fixture _fixture = new();

    [Test]
    public async Task Then_Pending_And_Approved_Applications_Are_Rejected_And_Pledge_Is_Closed()
    {
        // Arrange
        var command = new CleanupPledgeForNonLevyCommand { AccountId = 123, PledgeId = 999 };

        var pending = new GetApplicationsResponse
        {
            Applications =
            [
                new GetApplicationsResponse.Application { Id = 1 },
                new GetApplicationsResponse.Application { Id = 2 }
            ]
        };

        var approved = new GetApplicationsResponse
        {
            Applications =
            [
                new GetApplicationsResponse.Application { Id = 3 }
            ]
        };

        var service = new Mock<ILevyTransferMatchingService>();
        service.Setup(x => x.GetPledge(command.PledgeId)).ReturnsAsync(new Pledge { AccountId = command.AccountId, Status = PledgeStatus.Active });

        service.Setup(x => x.GetApplications(It.Is<GetApplicationsRequest>(r => r.PledgeId == command.PledgeId && r.ApplicationStatusFilter == ApplicationStatus.Pending)))
            .ReturnsAsync(pending);
        service.Setup(x => x.GetApplications(It.Is<GetApplicationsRequest>(r => r.PledgeId == command.PledgeId && r.ApplicationStatusFilter == ApplicationStatus.Approved)))
            .ReturnsAsync(approved);

        service.Setup(x => x.UndoApplicationApproval(It.IsAny<UndoApplicationApprovalRequest>()))
            .ReturnsAsync(new ApiResponse<UndoApplicationApprovalRequest>(new UndoApplicationApprovalRequest(command.PledgeId, 3), HttpStatusCode.OK, string.Empty));

        service.Setup(x => x.ClosePledge(It.IsAny<ClosePledgeRequest>()))
            .ReturnsAsync(new ApiResponse<ClosePledgeRequest>(new ClosePledgeRequest(command.PledgeId, new ClosePledgeRequest.ClosePledgeRequestData()), HttpStatusCode.OK, string.Empty));

        service.Setup(x => x.RejectApplication(It.IsAny<RejectApplicationRequest>())).Returns(Task.CompletedTask);

        var handler = new CleanupPledgeForNonLevyCommandHandler(service.Object, Mock.Of<ILogger<CleanupPledgeForNonLevyCommandHandler>>());

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        service.Verify(x => x.RejectApplication(It.IsAny<RejectApplicationRequest>()), Times.Exactly(3));
        service.Verify(x => x.UndoApplicationApproval(It.Is<UndoApplicationApprovalRequest>(r => r.PostUrl.Contains("/undo-approval"))), Times.Once);
        service.Verify(x => x.ClosePledge(It.IsAny<ClosePledgeRequest>()), Times.Once);
    }

    [Test]
    public async Task Then_No_Action_When_Pledge_Is_Already_Closed()
    {
        // Arrange
        var command = _fixture.Create<CleanupPledgeForNonLevyCommand>();

        var service = new Mock<ILevyTransferMatchingService>();
        service.Setup(x => x.GetPledge(command.PledgeId)).ReturnsAsync(new Pledge { AccountId = command.AccountId, Status = PledgeStatus.Closed });

        var handler = new CleanupPledgeForNonLevyCommandHandler(service.Object, Mock.Of<ILogger<CleanupPledgeForNonLevyCommandHandler>>());

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        service.Verify(x => x.GetApplications(It.IsAny<GetApplicationsRequest>()), Times.Never);
        service.Verify(x => x.ClosePledge(It.IsAny<ClosePledgeRequest>()), Times.Never);
    }
}
