using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetApplications
{
    public class GetApplicationsQuery : IRequest<GetApplicationsResult>
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
    }
}
