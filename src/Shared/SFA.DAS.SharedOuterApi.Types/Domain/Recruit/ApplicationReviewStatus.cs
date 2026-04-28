using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Types.Domain.Recruit;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApplicationReviewStatus : short
{
    New = 0,
    Successful = 1,
    Unsuccessful = 2,
    Shared = 3, // Indicates that the application has been newly shared
    InReview = 4,
    Interviewing = 5,
    EmployerInterviewing = 6,
    EmployerUnsuccessful = 7,
    PendingShared = 8,
    PendingToMakeUnsuccessful = 9,
    AllShared = 10, // Indicates that the application has been shared with all employers dateSharedWithEmployer is not null
}