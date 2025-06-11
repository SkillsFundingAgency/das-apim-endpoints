﻿using System;
using MediatR;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Command.PatchApplicationReview
{
    public sealed record PatchApplicationReviewCommand(Guid Id,
        string Status,
        string? TemporaryReviewStatus,
        string? EmployerFeedback,
        bool HasEverBeenEmployerInterviewing,
        DateTime? DateSharedWithEmployer) : IRequest;
}