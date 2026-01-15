using MediatR;
using SFA.DAS.LearnerData.Responses;

namespace SFA.DAS.LearnerData.Application.GetProviderRelationships;

public class GetProviderRelationshipQuery : IRequest<GetProviderRelationshipQueryResponse?>
{
    public required int Ukprn { get; set; }
}