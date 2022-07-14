using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Locations.GetLocations
{
    public class GetLocationsQuery : IRequest<GetLocationsQueryResponse>
    {
        public string SearchTerm { get ; set ; }
    }
}