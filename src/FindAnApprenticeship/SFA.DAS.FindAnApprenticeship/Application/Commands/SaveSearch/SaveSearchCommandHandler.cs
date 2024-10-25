using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.Domain.Configuration;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch;

public class SaveSearchCommandHandler(IFindApprenticeshipApiClient<FindAnApprenticeshipConfiguration> findApprenticeshipApiClient) : IRequestHandler<SaveSearchCommand, SaveSearchCommandResult>
{
    public async Task<SaveSearchCommandResult> Handle(SaveSearchCommand request, CancellationToken cancellationToken)
    {
        var payload = JsonConvert.SerializeObject(new SavedSearchParameters(
            request.SearchTerm,
            request.Location,
            request.Distance,
            request.SortOrder,
            request.DisabilityConfident,
            request.SelectedLevelIds,
            request.SelectedRouteIds
        ));

        var response = await findApprenticeshipApiClient.PostWithResponseCode<PostSavedSearchApiResponse>(
            new PostSavedSearchApiRequest(new PostSavedSearchApiRequestData(request.CandidateId, payload)));
        
        return response is not null && response.StatusCode == HttpStatusCode.Created
            ? SaveSearchCommandResult.From(response.Body)
            : SaveSearchCommandResult.None;
    }
}