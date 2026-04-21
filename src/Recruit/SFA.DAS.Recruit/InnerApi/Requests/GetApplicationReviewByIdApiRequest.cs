using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.Recruit.InnerApi.Requests;
public record GetApplicationReviewByIdApiRequest(Guid ApplicationReviewId)
    : IGetApiRequest
{
    public string GetUrl => $"api/applicationReviews/{ApplicationReviewId}";
}