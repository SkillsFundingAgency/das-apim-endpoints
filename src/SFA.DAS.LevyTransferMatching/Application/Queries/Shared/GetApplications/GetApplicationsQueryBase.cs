using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Shared.GetApplications
{
    public abstract class GetApplicationsQueryBase
    {
        public int? PledgeId { get; set; }
        public long? AccountId { get; set; }
    }
}
