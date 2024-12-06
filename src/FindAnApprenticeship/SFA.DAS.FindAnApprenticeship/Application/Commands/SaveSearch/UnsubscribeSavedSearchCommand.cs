using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch;

public record UnsubscribeSavedSearchCommand(Guid Id) : IRequest;
