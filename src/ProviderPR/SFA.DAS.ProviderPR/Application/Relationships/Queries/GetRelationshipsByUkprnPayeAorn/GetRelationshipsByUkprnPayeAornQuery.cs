using MediatR;

namespace SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationshipsByUkprnPayeAorn;
public class GetRelationshipsByUkprnPayeAornQuery : IRequest<GetRelationshipsByUkprnPayeAornResult>
{
    public long Ukprn { get; }
    public string Aorn { get; }
    public string Paye { get; set; }

    public GetRelationshipsByUkprnPayeAornQuery(long ukprn, string aorn, string paye)
    {
        Ukprn = ukprn;
        Aorn = aorn;
        Paye = paye;
    }
}
