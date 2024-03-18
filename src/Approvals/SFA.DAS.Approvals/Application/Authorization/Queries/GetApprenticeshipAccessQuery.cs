using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;

namespace SFA.DAS.Approvals.Application.Authorization.Queries;

public record GetApprenticeshipAccessQuery(Party Party, long PartyId, long ApprenticeshipId) : IRequest<bool> { }