using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetPostcodes;

public class GetPostcodeQueryResult
{
    public CoordinatesItem Coordinates { get; set; } = new();

    public static implicit operator GetPostcodeQueryResult(GetAddressesListItem addressesListItem)
    {
        if (addressesListItem == null || addressesListItem.Latitude == null || addressesListItem.Longitude == null) return null!;

        var result = new GetPostcodeQueryResult();
        result.Coordinates.Latitude = addressesListItem.Latitude.GetValueOrDefault();
        result.Coordinates.Longitude = addressesListItem.Longitude.GetValueOrDefault();

        return result;
    }
}
