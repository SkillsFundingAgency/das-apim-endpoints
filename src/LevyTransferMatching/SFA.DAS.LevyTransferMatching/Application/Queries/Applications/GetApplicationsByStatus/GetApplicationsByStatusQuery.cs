using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplicationsByStatus
{
    public class GetApplicationsByStatusQuery : IRequest<GetApplicationsByStatusResult>
    {
        public long AccountId { get; set; }

        public string ApplicationStatus { get; set; }

    }
}
