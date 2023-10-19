using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetLocations
{
    public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, GetLocationsQueryResponse>
    {
        private readonly ILocationService _locationService;

        public GetLocationsQueryHandler(ILOcationService)
        {
            
        }
        public Task<GetLocationsQueryResponse> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
        {
        }
    }
}
