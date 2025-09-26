#nullable enable
using System;

namespace SFA.DAS.Recruit.Api.Models
{
    public sealed record PostApplicationReviewApiRequest
    {
        public bool HasEverBeenEmployerInterviewing { get; init; }
        public DateTime? DateSharedWithEmployer { get; init; }
        public string? EmployerFeedback { get; init; } = null;
        public required string Status { get; init; }
        public string? TemporaryReviewStatus { get; init; } = null;
    }
}