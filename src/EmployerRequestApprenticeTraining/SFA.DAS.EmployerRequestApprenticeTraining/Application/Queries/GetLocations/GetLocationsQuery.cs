using MediatR;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocations
{
    public class GetLocationsQuery : IRequest<GetLocationsQueryResult>
    {
        public string SearchTerm { get; set; }
    }
}