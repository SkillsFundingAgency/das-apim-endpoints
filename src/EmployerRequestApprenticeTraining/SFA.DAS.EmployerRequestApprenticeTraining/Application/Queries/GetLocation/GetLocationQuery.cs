using MediatR;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation
{
    public class GetLocationQuery : IRequest<GetLocationQueryResult>
    {
        public string ExactMatch { get; set; }
    }
}