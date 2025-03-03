using MediatR;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocations
{
    public class GetLocationsQuery : IRequest<GetLocationsResult>
    {
        public string SearchTerm { get; set; }
    }
}