using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;

namespace SFA.DAS.SharedOuterApi.Services;

public class LocationLookupService(ILocationApiClient<LocationApiConfiguration> locationApiClient)
    : ILocationLookupService
{
    private const string PostcodeRegex = @"^[A-Za-z]{1,2}\d[A-Za-z\d]?\s*\d[A-Za-z]{2}$";
    private const string OutcodeRegex = @"^[A-Za-z]{1,2}\d[A-Za-z\d]?";
    private const string OutcodeDistrictRegex = @"^[A-Za-z]{1,2}\d[A-Za-z\d]?\s[A-Za-z]*";
    private const double MinMatch = 1;

    private static readonly TimeSpan RegexTimeOut = TimeSpan.FromMilliseconds(500);

    public async Task<LocationItem> GetLocationInformation(string location, double lat, double lon, bool includeDistrictNameInPostcodeDisplayName = false)
    {
        if (string.IsNullOrWhiteSpace(location))
        {
            return null;
        }

        if (lat != 0 && lon != 0)
        {
            return new LocationItem(location, [lat, lon], string.Empty);
        }

        GetLocationsListItem getLocationsListItem  = null;
        
        if (Regex.IsMatch(location, PostcodeRegex, RegexOptions.None, RegexTimeOut))
        { 
            var result = await locationApiClient.Get<GetLocationByFullPostcodeRequestV2Response>(new GetLocationByFullPostcodeRequestV2(location));
            getLocationsListItem = result.ToGetLocationsListItem();
            getLocationsListItem.IncludeDistrictNameInPostcodeDisplayName = includeDistrictNameInPostcodeDisplayName;
            location = getLocationsListItem.DisplayName;
        }
        else if (Regex.IsMatch(location, OutcodeDistrictRegex, RegexOptions.None, RegexTimeOut))
        {
            getLocationsListItem = await locationApiClient.Get<GetLocationsListItem>(new GetLocationByOutcodeAndDistrictRequest(location.Split(' ').FirstOrDefault()));
            if (getLocationsListItem.Location != null)
            {
                location = getLocationsListItem.DisplayName;
            }
        }
        else if(Regex.IsMatch(location, OutcodeRegex, RegexOptions.None, RegexTimeOut))
        {
            getLocationsListItem = await locationApiClient.Get<GetLocationsListItem>(new GetLocationByOutcodeRequest(location));
        }
        else if (location.Split(",").Length >= 2)
        {
            
            var locationInformation = location.Split(",");
            var locationName = string.Join(",",locationInformation.Take(locationInformation.Length-1)).Trim();
            var authorityName = locationInformation.Last().Trim();
            getLocationsListItem = await locationApiClient.Get<GetLocationsListItem>(new GetLocationByLocationAndAuthorityName(locationName, authorityName));
        }
        
        if (location.Length >= 2 && getLocationsListItem?.Location == null)
        {
            var locations = await locationApiClient.Get<GetLocationsListResponse>(new GetLocationsQueryRequest(location));

            var locationsListItem = locations?.Locations?.FirstOrDefault();
            if (locationsListItem != null)
            {
                getLocationsListItem = locationsListItem;
                location = getLocationsListItem.DisplayName;    
            }
        }

        return getLocationsListItem?.Location != null
            ? new LocationItem(location, getLocationsListItem.Location.GeoPoint, getLocationsListItem.Country)
            : null;
    }

    public async Task<GetAddressesListResponse> GetExactMatchAddresses(string fullPostcode)
    {
        if (!Regex.IsMatch(fullPostcode, PostcodeRegex, RegexOptions.None, RegexTimeOut))
        {
            return null;
        }

        var response = await locationApiClient.GetWithResponseCode<GetAddressesListResponse>(new GetAddressesQueryRequest(fullPostcode, MinMatch));

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"Location api did not return a successful response when trying to get addresses for postcode {fullPostcode}");
        }

        return response.Body;
    }

    public async Task<PostcodeInfo?> GetPostcodeInfoAsync(string postcode)
    {
        var response = await locationApiClient.GetWithResponseCode<GetLookupPostcodeResponse>(new GetLookupPostcodeRequest(postcode));
        return response.StatusCode == HttpStatusCode.NotFound
            ? null
            : PostcodeInfo.From(response.Body);
    }
}