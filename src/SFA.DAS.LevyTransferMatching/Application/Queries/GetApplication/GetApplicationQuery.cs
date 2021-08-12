using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication
{
    public class GetApplicationQuery : IRequest<GetApplicationResult>
    {
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
    }
}