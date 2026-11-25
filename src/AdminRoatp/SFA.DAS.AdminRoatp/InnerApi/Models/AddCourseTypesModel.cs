using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.AdminRoatp.InnerApi.Models;

public class AddCourseTypesModel
{
    public IEnumerable<CourseType> CourseTypes { get; set; } = [];
    public string UserId { get; set; } = string.Empty;
    public string UserDisplayName { get; set; } = string.Empty;
}
