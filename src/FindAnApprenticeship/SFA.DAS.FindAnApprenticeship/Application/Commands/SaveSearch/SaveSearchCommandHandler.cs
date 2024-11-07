using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch;

public class SaveSearchCommandHandler(
    IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
    ILocationLookupService locationLookupService) : IRequestHandler<SaveSearchCommand, SaveSearchCommandResult>
{
    public async Task<SaveSearchCommandResult> Handle(SaveSearchCommand request, CancellationToken cancellationToken)
    {
        var location = await locationLookupService.GetLocationInformation(request.Location, default, default, false);
        var payload = new SearchParameters(
            request.SearchTerm,
            request.SelectedRouteIds,
            request.Distance,
            request.DisabilityConfident,
            request.SelectedLevelIds,
            location?.GeoPoint[0].ToString(CultureInfo.InvariantCulture),
            location?.GeoPoint[1].ToString(CultureInfo.InvariantCulture)
        );

        var response = await findApprenticeshipApiClient.PostWithResponseCode<PostSavedSearchApiResponse>(
            new PostSavedSearchApiRequest(new PostSavedSearchApiRequestData(request.CandidateId, payload)));
        
        return response is not null && response.StatusCode == HttpStatusCode.OK
            ? SaveSearchCommandResult.From(response.Body)
            : SaveSearchCommandResult.None;
    }
}