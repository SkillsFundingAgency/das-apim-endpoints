using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetApprenticeshipsFilterValues;

public class GetApprenticeshipsFilterValuesQueryHandler(
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IMapper mapper)
        : IRequestHandler<GetApprenticeshipsFilterValuesQuery, GetApprenticeshipsFilterValuesQueryResult>
{
    public async Task<GetApprenticeshipsFilterValuesQueryResult> Handle(GetApprenticeshipsFilterValuesQuery request, CancellationToken cancellationToken)
    {
        var apprenticeshipResponse = await apiClient.GetWithResponseCode<GetApprenticeshipsFilterValuesResponse>(
            new GetApprenticeshipsFilterValuesRequest(request.ProviderId, request.EmployerAccountId));

        if (apprenticeshipResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        return mapper.Map<GetApprenticeshipsFilterValuesQueryResult>(apprenticeshipResponse.Body);
    }
}