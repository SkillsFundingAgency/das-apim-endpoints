namespace SFA.DAS.Approvals.Api.Models;

public class ProviderAccountDetailsResponse(int providerStatusTypeId)
{
    public int ProviderStatusTypeId { get; } = providerStatusTypeId;
}
