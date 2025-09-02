using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Extensions;

public static class AddressExtensions
{
    private const int PostcodeMinLength = 5;
    private const int InCodeLength = 3;
    
    public static string GetCity(this Address? address)
    {
        if (address is null)
        {
            return null;
        }

        // city should never be on first line
        List<string> lines = [
            address.AddressLine4,
            address.AddressLine3,
            address.AddressLine2
        ];
        
        return lines.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))?.Trim();
    }

    public static List<Address> OrderByCity(this List<Address> addresses)
    {
        return addresses
            .OrderBy(a => a.GetCity())
            .ToList();
    }

    public static string? ToSingleLineAddress(this Address? address)
    {
        if (address is null)
        {
            return null;
        }

        List<string?> lines =
        [
            address.AddressLine1,
            address.AddressLine2,
            address.AddressLine3,
            address.AddressLine4,
            address.Postcode
        ];

        return string.Join(", ", lines.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()));
    }
    
    private static string? PostcodeAsOutCode(this Address address)
    {
        var postcode = address.Postcode?.Replace(" ", "").Trim();

        return postcode is {Length: < PostcodeMinLength}
            ? postcode // If the length is less than InCodeLength it's already an outcode or empty/null
            : postcode?[..^InCodeLength];
    }

    public static void Anonymise(this Address address)
    {
        if (address is null)
        {
            return;
        }
            
        var city = address.GetCity();

        address.AddressLine1 = null;
        address.AddressLine2 = null;
        address.AddressLine3 = city;
        address.AddressLine4 = null;
        address.Postcode = address.PostcodeAsOutCode();
        address.Latitude = null;
        address.Longitude = null;
    }

    public static string? GetLastNonEmptyField(this Address address)
    {
        return new[]
        {
            address.AddressLine4,
            address.AddressLine3,
            address.AddressLine2,
            address.AddressLine1,
        }.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
    }
}