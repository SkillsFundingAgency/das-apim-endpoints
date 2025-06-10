using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Learners.Queries;

public class GetLearnerForProviderQueryHandler(
    IInternalApiClient<LearnerDataInnerApiConfiguration> learnerDataClient,
    ILogger<GetLearnerForProviderQueryHandler> logger)
    : IRequestHandler<GetLearnerForProviderQuery, GetLearnerForProviderQueryResult>
{
    public async Task<GetLearnerForProviderQueryResult> Handle(GetLearnerForProviderQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Learner Data {0}", request.LearnerId);
        var learnerDataResponse = await learnerDataClient.Get<GetLearnerForProviderResponse>(
            new GetLearnerForProviderRequest(
                request.ProviderId,
                request.LearnerId
            ));

        return learnerDataResponse;
    }
}