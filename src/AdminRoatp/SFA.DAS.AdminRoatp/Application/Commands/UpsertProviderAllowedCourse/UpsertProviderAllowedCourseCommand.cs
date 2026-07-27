using MediatR;

namespace SFA.DAS.AdminRoatp.Application.Commands.UpsertProviderAllowedCourse;

public class UpsertProviderAllowedCourseCommand : IRequest
{
    public string UserId { get; set; } = string.Empty;
    public string UserDisplayName { get; set; } = string.Empty;
    public DateTime? LastDateStarts { get; set; }
    public int Ukprn { get; set; }
    public string LarsCode { get; set; } = string.Empty;
}
