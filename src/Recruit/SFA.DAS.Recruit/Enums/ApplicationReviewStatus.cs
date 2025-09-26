using System.Text.Json.Serialization;

namespace SFA.DAS.Recruit.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ApplicationReviewStatus : short
    {
        New = 0,
        Successful = 1,
        Unsuccessful = 2,
        Shared = 3,//With new shared
        InReview = 4,
        Interviewing = 5,
        EmployerInterviewing = 6,
        EmployerUnsuccessful = 7,
        PendingShared = 8,
        PendingToMakeUnsuccessful = 9,
        AllShared = 10//all shared which is dateSharedWithEmployer is not null
    }
}
