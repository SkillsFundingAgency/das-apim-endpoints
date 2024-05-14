using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;

public class PostCreateApprenticeshipStartDateChangeRequest : IPostApiRequest
{
    public PostCreateApprenticeshipStartDateChangeRequest(
        Guid apprenticeshipKey,
        string initiator,
        string userId,
        DateTime actualStartDate,
        string reason)
    {
        ApprenticeshipKey = apprenticeshipKey;
        Data = new CreateApprenticeshipStartDateChangeRequest
        {
            Initiator = initiator,
            UserId = userId,
            ActualStartDate = actualStartDate,
            Reason = reason
        };
    }

    public Guid ApprenticeshipKey { get; set; }
    public string PostUrl => $"{ApprenticeshipKey}/startDateChange";
    public object Data { get; set; }
}

public class CreateApprenticeshipStartDateChangeRequest
{
    public string Initiator { get; set; }
    public string UserId { get; set; }
    public DateTime ActualStartDate { get; set; }
    public string Reason { get; set; }
}