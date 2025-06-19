#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Command.PatchApplicationReview
{
    public sealed record PatchApplicationReviewCommand(Guid Id,
        [Required] string Status,
        string? TemporaryReviewStatus,
        string? EmployerFeedback,
        bool HasEverBeenEmployerInterviewing,
        DateTime? DateSharedWithEmployer) : IRequest;
}