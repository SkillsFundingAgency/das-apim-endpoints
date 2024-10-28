using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetAccountDeletionQuery
{
    public record GetAccountDeletionQuery(Guid CandidateId) : IRequest<GetAccountDeletionQueryResult>;
}
