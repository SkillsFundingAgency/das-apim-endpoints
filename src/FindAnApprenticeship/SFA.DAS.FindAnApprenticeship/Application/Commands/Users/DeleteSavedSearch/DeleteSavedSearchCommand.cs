using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DeleteSavedSearch;

public record DeleteSavedSearchCommand(Guid CandidateId, Guid Id) : IRequest, IRequest<Unit>;