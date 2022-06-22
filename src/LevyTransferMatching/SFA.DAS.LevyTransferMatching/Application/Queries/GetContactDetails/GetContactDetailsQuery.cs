using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetContactDetails
{
    public class GetContactDetailsQuery : IRequest<GetContactDetailsResult>
    {
        public int OpportunityId { get; set; }
    }
}