using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DeleteSavedSearch;

public class DeleteSavedSearchCommandHandler(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient): IRequestHandler<DeleteSavedSearchCommand>
{
    public async Task Handle(DeleteSavedSearchCommand request, CancellationToken cancellationToken)
    {
        await findApprenticeshipApiClient.Delete(new DeleteCandidateSavedSearchRequest(request.CandidateId, request.Id));
    }
}