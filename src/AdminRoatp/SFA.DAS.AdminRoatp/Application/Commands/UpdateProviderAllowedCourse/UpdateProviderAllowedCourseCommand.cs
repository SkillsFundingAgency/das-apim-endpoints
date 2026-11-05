using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Models;

namespace SFA.DAS.AdminRoatp.Application.Commands.UpdateProviderAllowedCourse;

public class UpdateProviderAllowedCourseCommand : IRequest
{
    public int Ukprn { get; set; }
    public string LarsCode { get; set; } = string.Empty;
    public required PutProviderAllowedCourseModel RequestModel { get; set; }
}
