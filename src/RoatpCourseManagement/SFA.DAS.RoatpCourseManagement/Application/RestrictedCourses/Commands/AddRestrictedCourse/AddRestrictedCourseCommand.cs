using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.RestrictedCourses.Commands.AddRestrictedCourse;

public class AddRestrictedCourseCommand : IRequest
{
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
    public string LarsCode { get; set; }
}