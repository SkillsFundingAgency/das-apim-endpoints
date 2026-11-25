using SFA.DAS.AdminRoatp.Application.Commands.AddRestrictedCourse;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public class AddRestrictedCourseRequest : IPostApiRequest
{
    public string PostUrl => $"restricted-courses";
    public object Data { get; set; }
    public AddRestrictedCourseRequest(AddRestrictedCourseCommand data)
    {
        Data = data;
    }
}
