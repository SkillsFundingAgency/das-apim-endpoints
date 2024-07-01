using MediatR;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation
{
    public class GetLocationQuery : IRequest<GetLocationResult>
    {
        public string ExactSearchTerm { get; set; }
    }
}