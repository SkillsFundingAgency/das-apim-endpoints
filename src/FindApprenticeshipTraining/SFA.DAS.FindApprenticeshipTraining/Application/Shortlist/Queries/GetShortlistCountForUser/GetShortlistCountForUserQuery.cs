using System;
using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistCountForUser;

public record GetShortlistCountForUserQuery(Guid UserId) : IRequest<GetShortlistCountForUserQueryResult>;
