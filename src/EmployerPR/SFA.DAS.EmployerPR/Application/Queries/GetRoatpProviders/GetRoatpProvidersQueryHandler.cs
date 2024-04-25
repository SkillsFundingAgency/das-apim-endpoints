using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerPR.Application.Queries.GetRoatpProviders;
public class GetRoatpProvidersQueryHandler : IRequestHandler<GetRoatpProvidersQuery, GetRoatpProvidersQueryResult>
{
    private readonly IRoatpV2TrainingProviderService _roatpV2TrainingProviderService;

    public GetRoatpProvidersQueryHandler(IRoatpV2TrainingProviderService roatpV2TrainingProviderService)
    {
        _roatpV2TrainingProviderService = roatpV2TrainingProviderService;
    }


    public async Task<GetRoatpProvidersQueryResult> Handle(GetRoatpProvidersQuery request, CancellationToken cancellationToken)
    {
        var results = await _roatpV2TrainingProviderService.GetProviders(cancellationToken);
        var providers = results.RegisteredProviders.Select(provider => (RoatpProvider)provider);
        return new GetRoatpProvidersQueryResult { Providers = providers };
    }
}
