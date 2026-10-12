using MediatR;

namespace SFA.DAS.AdminRoatp.Application.Commands.RemoveProviderRestrictedApprenticeship;

public class RemoveProviderRestrictedApprenticeshipCommand : IRequest
{
    public int Ukprn { get; set; }
    public string LarsCode { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserDisplayName { get; set; } = string.Empty;
}
