using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.ProviderPermissions.Queries;

public record GetAccountProviderLegalEntitiesQuery(int Ukprn) : IRequest<GetProviderAccountLegalEntitiesResponse>;
