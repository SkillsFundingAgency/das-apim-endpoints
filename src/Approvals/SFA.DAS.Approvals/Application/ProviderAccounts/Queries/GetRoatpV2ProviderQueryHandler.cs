using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Polly.Registry;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.ProviderAccounts.Queries;

public class GetRoatpV2ProviderQueryHandler(IRoatpV2TrainingProviderService roatpV2TrainingProviderService, ResiliencePipelineProvider<string> pipelineProvider) : IRequestHandler<GetRoatpV2ProviderQuery, bool>
{
    public async Task<bool> Handle(GetRoatpV2ProviderQuery request, CancellationToken cancellationToken)
    {
        var pipeline = pipelineProvider.GetPipeline("default");

        var result = await pipeline.ExecuteAsync(async ct => await roatpV2TrainingProviderService.GetProviderSummary(request.Ukprn));

        return result != null && result.CanAccessApprenticeshipService;
    }
}