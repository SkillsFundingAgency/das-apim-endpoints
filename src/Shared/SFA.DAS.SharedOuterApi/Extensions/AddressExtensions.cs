using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Extensions;

public static class AddressExtensions
{
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
}