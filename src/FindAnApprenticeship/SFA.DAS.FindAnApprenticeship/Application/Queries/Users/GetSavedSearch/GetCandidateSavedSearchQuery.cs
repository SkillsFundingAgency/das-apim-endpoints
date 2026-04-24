using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearch;

public record GetCandidateSavedSearchQuery(Guid CandidateId, Guid Id) : IRequest<GetCandidateSavedSearchQueryResult>;