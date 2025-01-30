using MediatR;

namespace SFA.DAS.Approvals.Application.AgreementNotSigned.Queries;

public class GetAgreementNotSignedQuery : IRequest<GetAgreementNotSignedResult>
{
    public long AccountId { get; set; }
}
