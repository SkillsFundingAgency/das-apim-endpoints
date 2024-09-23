using MediatR;

namespace SFA.DAS.EmployerPR.Application.Relationships.Queries.GetRelationships;

public class GetRelationshipsQuery : IRequest<GetRelationshipsResponse>
{
    public long? Ukprn { get; set; }
    public int? AccountLegalEntityId { get; set; }
}