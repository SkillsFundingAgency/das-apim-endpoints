namespace SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetAddresses;

public class GetAddressesQueryResult
{
    public IEnumerable<AddressItem> Addresses { get; set; } = Enumerable.Empty<AddressItem>();
}
