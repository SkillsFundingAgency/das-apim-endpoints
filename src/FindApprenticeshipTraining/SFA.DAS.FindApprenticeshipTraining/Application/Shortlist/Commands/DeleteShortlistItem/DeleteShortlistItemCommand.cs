using System;
using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistItem;

public record DeleteShortlistItemCommand(Guid ShortlistId) : IRequest<DeleteShortlistItemCommandResult>;
