using MediatR;
using SFA.DAS.LearnerData.Responses;

namespace SFA.DAS.LearnerData.Application.GetProviderRelationships;

public class GetAllProviderRelationshipQuery : PagedQuery, IRequest<GetAllProviderRelationshipQueryResponse>
{
    public bool IsPaged => Page > 0 && PageSize.HasValue;
}