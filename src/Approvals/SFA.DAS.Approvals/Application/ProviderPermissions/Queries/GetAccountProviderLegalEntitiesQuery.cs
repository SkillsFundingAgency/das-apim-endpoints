using MediatR;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ProviderRelationships;

namespace SFA.DAS.Approvals.Application.ProviderPermissions.Queries;

public record GetAccountProviderLegalEntitiesQuery(int Ukprn) : IRequest<GetProviderAccountLegalEntitiesResponse>;
