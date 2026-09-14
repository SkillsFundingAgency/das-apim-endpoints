using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public class RestrictProviderRequest : IPostApiRequest
{
    public int Ukprn { get; set; }
    public CourseType CourseType { get; set; }
    public string PostUrl => $"providers/{Ukprn}/course-types/{CourseType}/restrict";
    public object Data { get; set; }

    public RestrictProviderRequest(int ukprn, CourseType courseType, RestrictProviderModel data)
    {
        Ukprn = ukprn;
        CourseType = courseType;
        Data = data;
    }
}
