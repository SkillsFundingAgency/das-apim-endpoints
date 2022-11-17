using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetAccountMinimumSignedAgreementVersion
{
    public class GetAccountMinimumSignedAgreementVersionQuery : IRequest<GetAccountMinimumSignedAgreementVersionQueryResult>
    {
        public long AccountId { get; set; }
    }
}
