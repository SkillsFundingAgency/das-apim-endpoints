using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetLocations
{
    public class GetLocationsQuery : IRequest<GetLocationsResult>
    {
        public string SearchTerm { get; set; }
    }
}
