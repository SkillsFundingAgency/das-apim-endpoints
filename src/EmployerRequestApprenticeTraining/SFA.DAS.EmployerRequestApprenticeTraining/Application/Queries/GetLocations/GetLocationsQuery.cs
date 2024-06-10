using MediatR;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocations
{
    public class GetLocationsQuery : IRequest<GetLocationsQueryResponse>
    {
        public string SearchTerm { get; set; }
    }
}