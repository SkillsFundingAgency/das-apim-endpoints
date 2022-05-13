using MediatR;

namespace SFA.DAS.Reservations.Application.Providers.Queries.GetCohort
{
    public class GetCohortQuery : IRequest<GetCohortResult>
    {
        public long CohortId { get; set; }
    }
}