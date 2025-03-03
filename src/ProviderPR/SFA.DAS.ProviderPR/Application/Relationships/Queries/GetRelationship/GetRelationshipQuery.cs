using MediatR;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationship;

public record GetRelationshipQuery(long Ukprn, long AccountLegalEntityId) : IRequest<GetRelationshipResponse>;
