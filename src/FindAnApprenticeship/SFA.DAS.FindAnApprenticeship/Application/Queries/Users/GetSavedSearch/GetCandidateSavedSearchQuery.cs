using System;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearches;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearch;

public record GetCandidateSavedSearchQuery(Guid CandidateId, Guid Id) : IRequest<GetCandidateSavedSearchQueryResult>;