using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Extensions
{
    public static class EmailTemplateAddressExtension
    {
        public static string GetEmploymentLocations(List<Address> addresses)
        {
            var firstLocation = addresses.FirstOrDefault();

            return firstLocation is null
                ? string.Empty
                : $"{GetOneLocationCityName(firstLocation)} and {addresses.Count - 1} other available locations";
        }

        public static string GetOneLocationCityName(Address? address)
        {
            if (address is null) return string.Empty;
            var city = address.GetLastNonEmptyField();
            return string.IsNullOrWhiteSpace(city) ? address.Postcode! : $"{city} ({address.Postcode})";
        }

        public static string GetEmploymentLocationCityNames(List<Address> addresses)
        {
            var cityNames = addresses
                .Select(address => address.GetLastNonEmptyField())
                .OfType<string>()
                .Distinct()
                .OrderBy(city => city)
                .ToList();

            return cityNames.Count == 1 && addresses.Count > 1
                ? $"{cityNames.First()} ({addresses.Count} available locations)"
                : string.Join(", ", cityNames);
        }
    }
}