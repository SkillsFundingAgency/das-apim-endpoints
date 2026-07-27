using MediatR;

namespace SFA.DAS.AdminRoatp.Application.Commands.AddRestrictedCourse;

public class AddRestrictedCourseCommand : IRequest
{
    public string UserId { get; set; } = string.Empty;
    public string UserDisplayName { get; set; } = string.Empty;
    public string LarsCode { get; set; } = string.Empty;
}
