using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.RecruitV2Api.Requests;

public class PostCreateApplicationReviewNotificationsRequest(Guid applicationId) : IPostApiRequest
{
    public string PostUrl => $"api/applicationreviews/{applicationId}/create-notifications";
    public object Data { get; set; }
}