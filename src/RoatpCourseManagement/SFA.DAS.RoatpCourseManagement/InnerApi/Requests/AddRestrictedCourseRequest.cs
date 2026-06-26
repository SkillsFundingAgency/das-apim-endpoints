using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.RoatpCourseManagement.Application.RestrictedCourses.Commands.AddRestrictedCourse;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class AddRestrictedCourseRequest : IPostApiRequest
{
    public string PostUrl => $"restricted-courses";
    public object Data { get; set; }
    public AddRestrictedCourseRequest(AddRestrictedCourseCommand data)
    {
        Data = data;
    }
}
