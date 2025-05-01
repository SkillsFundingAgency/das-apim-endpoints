using System;
using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistItem;

public class DeleteShortlistItemCommand : IRequest<Unit>
{
    public Guid ShortlistId { get; set; }
}