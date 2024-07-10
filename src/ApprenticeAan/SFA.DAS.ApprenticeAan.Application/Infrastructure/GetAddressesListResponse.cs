namespace SFA.DAS.ApprenticeAan.Application.Infrastructure;
public class GetAddressesListResponse
{
    public IEnumerable<GetAddressesListItem> Addresses { get; set; } = Enumerable.Empty<GetAddressesListItem>();
}
