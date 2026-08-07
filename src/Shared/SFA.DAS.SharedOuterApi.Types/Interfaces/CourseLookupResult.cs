using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.SharedOuterApi.Types.Interfaces;

public enum CourseLookupStatus
{
    Found,
    NotFound,
    Unavailable
}

public record CourseLookupResult(CourseLookupStatus Status, CourseLookupDetailResponse? Course);
