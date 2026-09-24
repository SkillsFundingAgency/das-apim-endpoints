using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.InvalidIlrChanges.Queries;

public class GetInvalidIlrChangesQuery(
    long providerId,
    long apprenticeshipId,
    string innerPath = GetInvalidIlrChangesRequest.InvalidIlrChangesPath) : IRequest<GetInvalidIlrChangesResponse>
{
    public long ProviderId { get; } = providerId;
    public long ApprenticeshipId { get; } = apprenticeshipId;
    public string InnerPath { get; } = innerPath;
}
