#nullable enable
using MediatR;
using System;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Command.PatchApplicationReview
{
    public sealed record PatchApplicationReviewCommand(Guid Id,
        string Status,
        string? TemporaryReviewStatus,
        string? EmployerFeedback,
        bool HasEverBeenEmployerInterviewing,
        DateTime? DateSharedWithEmployer, 
        string? CandidateFeedback) : IRequest;
}