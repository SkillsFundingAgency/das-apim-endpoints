using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerPR.Application.RoatpProviders.Queries.GetRoatpProviders;
public class GetRoatpProvidersQueryHandler(IRoatpV2TrainingProviderService _roatpV2TrainingProviderService) : IRequestHandler<GetRoatpProvidersQuery, GetRoatpProvidersQueryResult>
{
    public async Task<GetRoatpProvidersQueryResult> Handle(GetRoatpProvidersQuery request, CancellationToken cancellationToken)
    {
        var results = await _roatpV2TrainingProviderService.GetProviders(cancellationToken);
        var providers = results.RegisteredProviders.Select(provider => (RoatpProvider)provider);
        return new GetRoatpProvidersQueryResult { Providers = providers };
    }
}