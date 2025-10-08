using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
public record UpdateCourseTypesRequest(int ukprn, UpdateCourseTypesModel data) : IPutApiRequest
{
    public string PutUrl => $"/organisations/{ukprn}/allowed-short-courses";

    public object Data { get; set; } = data;
}