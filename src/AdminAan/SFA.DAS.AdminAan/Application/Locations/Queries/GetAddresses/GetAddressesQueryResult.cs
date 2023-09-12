namespace SFA.DAS.AdminAan.Application.Locations.Queries.GetAddresses;

public class GetAddressesQueryResult
{
    public IEnumerable<AddressItem> Addresses { get; set; } = Enumerable.Empty<AddressItem>();
}