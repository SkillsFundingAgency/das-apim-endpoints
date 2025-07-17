using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteApplication;

public record DeleteApplicationCommand(Guid CandidateId, Guid ApplicationId): IRequest<DeleteApplicationCommandResult>;