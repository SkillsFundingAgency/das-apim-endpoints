using SFA.DAS.DigitalCertificates.Application.Queries.GetLocations;
using System.Collections.Generic;
using System.Linq;


namespace SFA.DAS.DigitalCertificates.Api.Models.Locations
{
    public class GetLocationsResponse
    {
        public AddressesDto Addresses { get; set; }

        public static implicit operator GetLocationsResponse(GetLocationsResult source)
        {
            if (source == null) return null;
            return new GetLocationsResponse
            {
                Addresses = source.Addresses == null ? null : new AddressesDto
                {
                    Addresses = source.Addresses.Addresses == null ? new List<AddressItemDto>() : source.Addresses.Addresses.Select(a => new AddressItemDto
                    {
                        Uprn = a.Uprn,
                        Organisation = a.Organisation,
                        Premises = a.Premises,
                        Thoroughfare = a.Thoroughfare,
                        Locality = a.Locality,
                        PostTown = a.PostTown,
                        County = a.County,
                        Postcode = a.Postcode,
                        Country = a.Country,
                        AddressLine1 = a.AddressLine1,
                        AddressLine2 = a.AddressLine2,
                        AddressLine3 = a.AddressLine3,
                        Longitude = a.Longitude,
                        Latitude = a.Latitude,
                        Match = a.Match
                    }).ToList()
                }
            };
        }
    }

    public class AddressesDto
    {
        public IEnumerable<AddressItemDto> Addresses { get; set; } = new List<AddressItemDto>();
    }

    public class AddressItemDto
    {
        public string Uprn { get; set; }
        public string Organisation { get; set; }
        public string Premises { get; set; }
        public string Thoroughfare { get; set; }
        public string Locality { get; set; }
        public string PostTown { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? Match { get; set; }
    }
}
