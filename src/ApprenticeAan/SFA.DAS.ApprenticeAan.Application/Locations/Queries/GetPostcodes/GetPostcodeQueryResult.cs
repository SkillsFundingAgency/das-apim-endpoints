using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetPostcodes;

public class GetPostcodeQueryResult
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public static implicit operator GetPostcodeQueryResult(GetAddressesListItem addressesListItem)
    {
        if (addressesListItem == null) return null!;

        var result = new GetPostcodeQueryResult();
        result.Latitude = addressesListItem.Latitude.GetValueOrDefault();
        result.Longitude = addressesListItem.Longitude.GetValueOrDefault();

        return result;
    }
}
