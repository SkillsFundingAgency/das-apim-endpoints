using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.RoatpCourseManagement.Application.RestrictedCourses.Commands;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class AddRestrictedCourseRequest : IPostApiRequest
{
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
    public string PostUrl => $"restricted-courses";
    public object Data { get; set; }
    public AddRestrictedCourseRequest(AddRestrictedCourseCommand data)
    {
        UserId = data.UserId;
        UserDisplayName = data.UserDisplayName;
        Data = data;
    }
}
