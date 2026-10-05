using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public class GetAllStandardsRequest : IGetApiRequest
{
    public string GetUrl => $"standards?courseType={CourseType}";
    public CourseType? CourseType { get; set; }

    public GetAllStandardsRequest(CourseType? courseType)
    {
        CourseType = courseType;
    }
}
