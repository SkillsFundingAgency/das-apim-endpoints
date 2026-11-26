using MediatR;

namespace SFA.DAS.AdminRoatp.Application.Commands.UpdateProviderRestrictedApprenticeship;

public class UpdateProviderRestrictedApprenticeshipCommand : IRequest
{
    public int Ukprn { get; set; }
    public string LarsCode { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserDisplayName { get; set; } = string.Empty;
    public DateTime? LastDateStarts { get; set; }
}
