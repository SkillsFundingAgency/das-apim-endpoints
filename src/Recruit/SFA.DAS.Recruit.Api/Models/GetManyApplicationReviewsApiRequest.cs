using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Api.Models;

public record GetManyApplicationReviewsApiRequest
{
    public List<Guid> ApplicationReviewIds { get; init; } = new();
}
