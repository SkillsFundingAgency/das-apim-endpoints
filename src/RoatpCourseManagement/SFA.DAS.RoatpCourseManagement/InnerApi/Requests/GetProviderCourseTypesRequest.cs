using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class GetProviderCourseTypesRequest : IGetApiRequest
{
    public string GetUrl => $"providers/{Ukprn}/course-types";
    public int Ukprn { get; }

    public GetProviderCourseTypesRequest(int ukprn)
    {
        Ukprn = ukprn;
    }
}