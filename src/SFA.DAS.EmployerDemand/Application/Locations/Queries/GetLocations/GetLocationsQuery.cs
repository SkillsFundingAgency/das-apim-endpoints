using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Locations.Queries.GetLocations
{
    public class GetLocationsQuery : IRequest<GetLocationsQueryResponse>
    {
        public string SearchTerm { get; set; }
    }
}