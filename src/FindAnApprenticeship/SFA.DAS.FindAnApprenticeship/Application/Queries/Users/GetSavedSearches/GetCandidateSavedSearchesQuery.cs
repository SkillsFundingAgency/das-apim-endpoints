using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearches;

public record GetCandidateSavedSearchesQuery(Guid CandidateId) : IRequest<GetCandidateSavedSearchesQueryResult>;