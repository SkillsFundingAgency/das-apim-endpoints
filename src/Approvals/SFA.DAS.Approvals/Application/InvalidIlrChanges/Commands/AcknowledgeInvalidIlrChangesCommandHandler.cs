using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.InvalidIlrChanges.Commands;

public class AcknowledgeInvalidIlrChangesCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient)
    : IRequestHandler<AcknowledgeInvalidIlrChangesCommand>
{
    public async Task Handle(AcknowledgeInvalidIlrChangesCommand command, CancellationToken cancellationToken)
    {
        var request = new PostInvalidIlrChangesRequest(command.ApprenticeshipId, new PostInvalidIlrChangesRequestData
        {
            ProviderId = command.ProviderId,
            UserInfo = command.UserInfo,
            Acknowledgements = command.Acknowledgements
        });

        var response = await commitmentsApiClient.PostWithResponseCode<NullResponse>(request, false);
        response.EnsureSuccessStatusCode();
    }
}
