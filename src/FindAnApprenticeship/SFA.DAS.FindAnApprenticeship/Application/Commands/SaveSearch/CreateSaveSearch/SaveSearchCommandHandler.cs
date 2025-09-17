using System;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch.CreateSaveSearch;

public class SaveSearchCommandHandler(
    IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
    ILocationLookupService locationLookupService) : IRequestHandler<SaveSearchCommand, SaveSearchCommandResult>
{
    public async Task<SaveSearchCommandResult> Handle(SaveSearchCommand request, CancellationToken cancellationToken)
    {
        var location = await locationLookupService.GetLocationInformation(request.Location, default, default, false);
        var response = await findApprenticeshipApiClient.PutWithResponseCode<PutSavedSearchApiResponse>(
            new PutSavedSearchApiRequest(request.CandidateId, request.Id, new PutSavedSearchApiRequestData
            {
                UnSubscribeToken = request.UnSubscribeToken,
                SearchParameters = new SearchParameters(request.SearchTerm,
                    request.SelectedRouteIds,
                    request.Distance,
                    request.DisabilityConfident,
                    request.ExcludeNational,
                    request.SelectedLevelIds,
                    request.Location,
                    location?.GeoPoint[0].ToString(CultureInfo.InvariantCulture),
                    location?.GeoPoint[1].ToString(CultureInfo.InvariantCulture),
                    request.ApprenticeshipTypes)
            }));

        return response is not null && response.StatusCode == HttpStatusCode.OK
            ? SaveSearchCommandResult.From(response.Body)
            : SaveSearchCommandResult.None;
    }
}