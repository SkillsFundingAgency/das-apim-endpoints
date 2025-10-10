using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Requests;
public record GetApplicationReviewsByIdsApiRequest(List<Guid> ApplicationIds) : IPostApiRequest
{
    public string PostUrl => "api/applicationReviews";
    public object Data { get; set; } = ApplicationIds;
}