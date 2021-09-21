using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplicationStatus
{
    public class GetApplicationStatusQuery : IRequest<GetApplicationStatusResult>
    {
        public int OpportunityId { get; set; }
        public int ApplicationId { get; set; }
    }
}