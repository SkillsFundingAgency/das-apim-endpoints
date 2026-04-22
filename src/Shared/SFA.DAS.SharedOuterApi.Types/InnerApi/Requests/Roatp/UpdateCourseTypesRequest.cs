using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;
public record UpdateCourseTypesRequest(int ukprn, UpdateCourseTypesModel data) : IPutApiRequest
{
    public string PutUrl => $"/organisations/{ukprn}/course-types";

    public object Data { get; set; } = data;
}