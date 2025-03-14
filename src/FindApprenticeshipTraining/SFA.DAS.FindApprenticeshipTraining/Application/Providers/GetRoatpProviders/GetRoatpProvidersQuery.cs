using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetRoatpProviders;

public class GetRoatpProvidersQuery : IRequest<GetRoatpProvidersQueryResult>
{
    public bool Live { get; set; } = false;
}
