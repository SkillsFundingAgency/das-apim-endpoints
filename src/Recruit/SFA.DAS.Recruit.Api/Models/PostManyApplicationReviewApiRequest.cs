#nullable enable
using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Api.Models
{
    public sealed record PostManyApplicationReviewApiRequest
    {
        public required List<Guid> ApplicationReviewIds { get; init; }
        public bool HasEverBeenEmployerInterviewing { get; init; }
        public DateTime? DateSharedWithEmployer { get; init; }
        public string? EmployerFeedback { get; init; } = null;
        public string? Status { get; init; }
        public string? TemporaryReviewStatus { get; init; } = null;
    }
}