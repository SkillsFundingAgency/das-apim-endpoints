using MediatR;

namespace SFA.DAS.EmployerPR.Application.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQuery : IRequest<GetEmployerRelationshipsQueryResult>
{
    public long AccountId { get; set; }

    public long? Ukprn { get; set; }

    public string? AccountlegalentityPublicHashedId { get; set; }

    public GetEmployerRelationshipsQuery(long accountId, long? ukprn = null, string? accountlegalentityPublicHashedId = null)
    {
        AccountId = accountId;
        Ukprn = ukprn;
        AccountlegalentityPublicHashedId = accountlegalentityPublicHashedId;
    }
}