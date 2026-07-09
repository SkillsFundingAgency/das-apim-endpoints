using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeships;

public class GetApprenticeshipsQueryHandler(
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IMapper mapper)
        : IRequestHandler<GetApprenticeshipsQuery, GetApprenticeshipsQueryResult>
{
    public async Task<GetApprenticeshipsQueryResult> Handle(GetApprenticeshipsQuery request, CancellationToken cancellationToken)
    {
        var apprenticeshipResponseTask = apiClient.GetWithResponseCode<GetApprenticeshipsResponse>(
            new GetApprenticeshipsRequest(request.ProviderId.Value, request));

        var filtersTask = apiClient.GetWithResponseCode<GetApprenticeshipsFilterValuesResponse>(
            new GetApprenticeshipsFilterValuesRequest(request.ProviderId, request.AccountId));

        await Task.WhenAll(apprenticeshipResponseTask, filtersTask);

        var apprenticeshipResponse = apprenticeshipResponseTask.Result;
        var filtersResponse = filtersTask.Result;

        if (apprenticeshipResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        if (filtersResponse.StatusCode == HttpStatusCode.NotFound)
        { return null; }

        var apprenticeShips = mapper.Map<GetApprenticeshipsQueryResult>(apprenticeshipResponse.Body);
        apprenticeShips.ApprenticeshipFiltersValue = mapper.Map<GetApprenticeshipsFilterValuesQueryResult>(filtersResponse.Body);
        return apprenticeShips;
    }
}