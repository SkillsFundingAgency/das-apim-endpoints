using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests;

public class PostCreateApplicationReviewNotificationsRequest(Guid applicationReviewId): IPostApiRequest
{
    public string PostUrl => $"api/applicationreviews/{applicationReviewId}/create-notifications";
    public object Data { get; set; }
}