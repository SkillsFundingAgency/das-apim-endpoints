using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.RecruitV2Api.Requests;

public class GetApplicationReviewByApplicationIdRequest(Guid applicationId) : IGetApiRequest
{
    public string GetUrl => $"api/applicationReviews?applicationId={applicationId}";
}