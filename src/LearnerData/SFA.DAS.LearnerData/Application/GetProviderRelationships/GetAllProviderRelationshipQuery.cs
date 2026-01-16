using MediatR;
using SFA.DAS.LearnerData.Responses;

namespace SFA.DAS.LearnerData.Application.GetProviderRelationships;

public class GetAllProviderRelationshipQuery : PagedQueryResult<GetAllProviderRelationshipQueryResponse?>, IRequest<GetAllProviderRelationshipQueryResponse?>
{
}