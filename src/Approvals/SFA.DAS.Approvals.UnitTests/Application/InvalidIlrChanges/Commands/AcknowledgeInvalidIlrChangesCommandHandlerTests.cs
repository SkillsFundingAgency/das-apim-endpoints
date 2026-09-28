using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Application.InvalidIlrChanges.Commands;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.InvalidIlrChanges.Commands;

public class AcknowledgeInvalidIlrChangesCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ThenPostsAcknowledgementsToInnerApi(
        AcknowledgeInvalidIlrChangesCommand command,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient)
    {
        apiClient.Setup(x => x.PostWithResponseCode<NullResponse>(It.IsAny<PostInvalidIlrChangesRequest>(), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.OK, string.Empty));

        var handler = new AcknowledgeInvalidIlrChangesCommandHandler(apiClient.Object);

        await handler.Handle(command, CancellationToken.None);

        apiClient.Verify(x => x.PostWithResponseCode<NullResponse>(
            It.Is<PostInvalidIlrChangesRequest>(request =>
                request.PostUrl == $"api/apprenticeships/{command.ApprenticeshipId}/{command.InnerPath}" &&
                ((PostInvalidIlrChangesRequestData)request.Data).ProviderId == command.ProviderId &&
                ((PostInvalidIlrChangesRequestData)request.Data).UserInfo == command.UserInfo &&
                ((PostInvalidIlrChangesRequestData)request.Data).Acknowledgements == command.Acknowledgements),
            false), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_ThenPostsAcknowledgementsToDeclinedChangesInnerPath(
        AcknowledgeInvalidIlrChangesCommand command,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient)
    {
        command.InnerPath = GetInvalidIlrChangesRequest.DeclinedChangesPath;
        apiClient.Setup(x => x.PostWithResponseCode<NullResponse>(It.IsAny<PostInvalidIlrChangesRequest>(), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.OK, string.Empty));

        var handler = new AcknowledgeInvalidIlrChangesCommandHandler(apiClient.Object);

        await handler.Handle(command, CancellationToken.None);

        apiClient.Verify(x => x.PostWithResponseCode<NullResponse>(
            It.Is<PostInvalidIlrChangesRequest>(request =>
                request.PostUrl == $"api/apprenticeships/{command.ApprenticeshipId}/declined-changes"),
            false), Times.Once);
    }
}
