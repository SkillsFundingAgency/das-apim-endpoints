using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.TrackProgressInternal.Apis.CoursesInnerApi;

public class GetKsbsForCourseOptionRequest : IGetApiRequest
{
    private readonly string _standardUId;
    private readonly string _option;

    public GetKsbsForCourseOptionRequest(string standardUId, string option)
    {
        _standardUId = standardUId;
        _option = option;
    }

    public string GetUrl => $"/api/courses/standards/{_standardUId}/options/{_option}/ksbs";
}