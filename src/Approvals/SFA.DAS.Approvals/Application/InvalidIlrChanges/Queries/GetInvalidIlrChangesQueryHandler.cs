using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Exceptions;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.InvalidIlrChanges.Queries;

public class GetInvalidIlrChangesQueryHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient,
    ServiceParameters serviceParameters)
    : IRequestHandler<GetInvalidIlrChangesQuery, GetInvalidIlrChangesResponse>
{
    public async Task<GetInvalidIlrChangesResponse> Handle(GetInvalidIlrChangesQuery query, CancellationToken cancellationToken)
    {
        var apprenticeshipResponse = await commitmentsApiClient.GetWithResponseCode<GetApprenticeshipResponse>(
            new GetApprenticeshipRequest(query.ApprenticeshipId));

        if (apprenticeshipResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        apprenticeshipResponse.EnsureSuccessStatusCode();

        var apprenticeship = apprenticeshipResponse.Body;
        if (apprenticeship == null)
        {
            throw new ResourceNotFoundException();
        }

        if (!apprenticeship.CheckParty(serviceParameters))
        {
            throw new UnauthorizedAccessException($"You do not have permissions to access apprenticeship {query.ApprenticeshipId}");
        }

        var invalidIlrChangesResponse = await commitmentsApiClient.GetWithResponseCode<GetInvalidIlrChangesResponse>(
            new GetInvalidIlrChangesRequest(query.ApprenticeshipId, query.ProviderId, query.InnerPath));

        if (invalidIlrChangesResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        invalidIlrChangesResponse.EnsureSuccessStatusCode();

        var body = invalidIlrChangesResponse.Body ?? new GetInvalidIlrChangesResponse();
        body.FirstName = apprenticeship.FirstName;
        body.LastName = apprenticeship.LastName;
        return body;
    }
}
