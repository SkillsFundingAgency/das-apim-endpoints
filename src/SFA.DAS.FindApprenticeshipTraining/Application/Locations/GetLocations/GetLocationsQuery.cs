using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Locations.GetLocations
{
    public class GetLocationsQuery : IRequest<GetLocationsResponse>
    {
        public string SearchTerm { get ; set ; }
    }
}