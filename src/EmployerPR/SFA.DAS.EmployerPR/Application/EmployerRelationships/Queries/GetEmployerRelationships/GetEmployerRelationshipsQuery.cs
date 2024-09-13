using MediatR;

namespace SFA.DAS.EmployerPR.Application.EmployerRelationships.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQuery : IRequest<GetEmployerRelationshipsQueryResult>
{
    public string AccountHashedId { get; set; }

    public long? Ukprn { get; set; }

    public string? AccountlegalentityPublicHashedId { get; set; }

    public GetEmployerRelationshipsQuery(string accountHashedId, long? ukprn = null, string? accountlegalentityPublicHashedId = null)
    {
        AccountHashedId = accountHashedId;
        Ukprn = ukprn;
        AccountlegalentityPublicHashedId = accountlegalentityPublicHashedId;
    }
}