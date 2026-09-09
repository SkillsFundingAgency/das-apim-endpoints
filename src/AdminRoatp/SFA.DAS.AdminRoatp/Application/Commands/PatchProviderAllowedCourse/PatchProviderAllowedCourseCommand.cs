using MediatR;

namespace SFA.DAS.AdminRoatp.Application.Commands.PatchProviderAllowedCourse;

public class PatchProviderAllowedCourseCommand : IRequest
{
    public string UserId { get; set; } = string.Empty;
    public string UserDisplayName { get; set; } = string.Empty;
    public int Ukprn { get; set; }
    public string LarsCode { get; set; } = string.Empty;
    public DateTime? LastDateStarts { get; set; }
}
