using MediatR;

namespace SFA.DAS.AdminRoatp.Application.Commands.AddProviderRestrictedApprenticeship;

public class AddProviderRestrictedApprenticeshipCommand : IRequest
{
    public int Ukprn { get; set; }
    public string LarsCode { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserDisplayName { get; set; } = string.Empty;
    public DateTime? LastDateStarts { get; set; }
}
