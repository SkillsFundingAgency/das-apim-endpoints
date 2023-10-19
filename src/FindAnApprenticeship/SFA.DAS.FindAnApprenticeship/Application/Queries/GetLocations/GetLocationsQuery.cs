using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetLocations
{
    public class GetLocationsQuery : IRequest<GetLocationsQueryResponse>
    {
        public string SearchTerm { get; set; }
    }
}
