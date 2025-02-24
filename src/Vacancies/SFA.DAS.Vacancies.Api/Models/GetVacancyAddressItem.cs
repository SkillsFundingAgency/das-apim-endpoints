using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.Models;

public class GetVacancyAddressItem
{
    /// <summary>
    /// First line of an address where the apprentice will work.
    /// </summary>
    public string AddressLine1 { get; set; }
    
    /// <summary>
    /// Second line of an address where the apprentice will work.
    /// </summary>
    public string AddressLine2 { get; set; }
    
    /// <summary>
    /// Third line of an address where the apprentice will work.
    /// </summary>
    public string AddressLine3 { get; set; }
    
    /// <summary>
    /// Fourth line of an address where the apprentice will work.
    /// </summary>
    public string AddressLine4 { get; set; }
    
    /// <summary>
    /// Postcode of an address where the apprentice will work.
    /// </summary>
    public string Postcode { get; set; }
    
    /// <summary>
    /// The latitude of the address where the apprentice will work.
    /// </summary>
    public double? Latitude { get; set; }
    
    /// <summary>
    /// The longitude of the address where the apprentice will work.
    /// </summary>
    public double? Longitude { get; set; }

    public static GetVacancyAddressItem From(Address source)
    {
        return source == null
            ? new GetVacancyAddressItem()
            : new GetVacancyAddressItem
                {
                    AddressLine1 = string.IsNullOrWhiteSpace(source.AddressLine1) ? null : source.AddressLine1,
                    AddressLine2 = string.IsNullOrWhiteSpace(source.AddressLine2) ? null : source.AddressLine2,
                    AddressLine3 = string.IsNullOrWhiteSpace(source.AddressLine3) ? null : source.AddressLine3,
                    AddressLine4 = string.IsNullOrWhiteSpace(source.AddressLine4) ? null : source.AddressLine4,
                    Postcode = source.Postcode,
                    Latitude = source.Latitude.ToGeoWithMetreAccuracy(),
                    Longitude = source.Longitude.ToGeoWithMetreAccuracy(),
                };
    }
}