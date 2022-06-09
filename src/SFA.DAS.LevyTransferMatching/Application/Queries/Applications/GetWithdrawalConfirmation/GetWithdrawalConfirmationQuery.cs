using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetWithdrawalConfirmation
{
    public class GetWithdrawalConfirmationQuery : IRequest<GetWithdrawalConfirmationQueryResult>
    {
        public int ApplicationId { get; set; }
    }
}
