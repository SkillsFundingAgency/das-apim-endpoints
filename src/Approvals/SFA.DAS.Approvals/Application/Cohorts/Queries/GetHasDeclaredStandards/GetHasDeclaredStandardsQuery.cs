using MediatR;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetHasDeclaredStandards
{
    public class GetHasDeclaredStandardsQuery : IRequest<GetHasDeclaredStandardsQueryResult>
    {
        public long? ProviderId { get; set; }
    }
}
