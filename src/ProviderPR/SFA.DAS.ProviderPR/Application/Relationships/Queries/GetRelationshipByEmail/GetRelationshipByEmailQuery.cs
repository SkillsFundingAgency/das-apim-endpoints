using MediatR;

namespace SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationshipByEmail;
public class GetRelationshipByEmailQuery : IRequest<GetRelationshipByEmailQueryResult>
{
    public long Ukprn { get; }
    public string Email { get; }

    public GetRelationshipByEmailQuery(string email, long ukprn)
    {
        Ukprn = ukprn;
        Email = email;
    }
}
