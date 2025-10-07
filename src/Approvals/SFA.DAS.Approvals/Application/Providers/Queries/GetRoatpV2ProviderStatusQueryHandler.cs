using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Providers.Queries;

public class GetRoatpV2ProviderStatusQueryHandler : IRequestHandler<GetRoatpV2ProviderStatusQuery, GetRoatpV2ProviderStatusQueryResult>
{
    private readonly IRoatpV2TrainingProviderService _roatpV2TrainingProviderService;

    public GetRoatpV2ProviderStatusQueryHandler(IRoatpV2TrainingProviderService roatpV2TrainingProviderService)
    {
        _roatpV2TrainingProviderService = roatpV2TrainingProviderService;
    }   

    public async Task<GetRoatpV2ProviderStatusQueryResult> Handle(GetRoatpV2ProviderStatusQuery request, CancellationToken cancellationToken)
    {
        var result = await _roatpV2TrainingProviderService.GetProviderSummary(request.Ukprn);

        return new GetRoatpV2ProviderStatusQueryResult(result.StatusId);
    }
}

