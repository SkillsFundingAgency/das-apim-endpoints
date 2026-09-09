namespace SFA.DAS.AdminRoatp.InnerApi.Models;

public class AddCourseTypesModel
{
    public IEnumerable<string> CourseTypes { get; set; }
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
}
