using MediatR;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.AdminRoatp.Application.Commands.RestrictProvider;

public class RestrictProviderCommand : IRequest
{
    public string UserId { get; set; } = string.Empty;
    public string UserDisplayName { get; set; } = string.Empty;
    public int Ukprn { get; set; }
    public CourseType CourseType { get; set; }
}
