using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Queries;

public class ValidateEmailOverlapQueryHandler : IRequestHandler<ValidateEmailOverlapQuery, ValidateEmailOverlapQueryResult>
{
    private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

    public ValidateEmailOverlapQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<ValidateEmailOverlapQueryResult> Handle(ValidateEmailOverlapQuery request, CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetWithResponseCode<ValidateEmailOverlapQueryResult>(
         new ValidateEmailOverlapRequest(request.DraftApprenticeshipId, request.StartDate, request.EndDate, request.Email, request.CohortId));
        response.EnsureSuccessStatusCode();

        return new ValidateEmailOverlapQueryResult
        {
            HasOverlapWithEmail = response.Body.HasOverlapWithEmail
        };
    }
}
