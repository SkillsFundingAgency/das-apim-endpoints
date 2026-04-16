using System;

namespace SFA.DAS.Recruit.Api.Models;

public record PostSharedApplicationReviewedEventModel
{
    public required Guid ApplicationReviewId { get; set; }
}