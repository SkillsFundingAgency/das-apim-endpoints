namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

public class GetAddressesListResponse
{
    public IEnumerable<GetAddressesListItem> Addresses { get; set; } = [];
}