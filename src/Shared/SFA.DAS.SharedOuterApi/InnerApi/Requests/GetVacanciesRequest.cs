using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Web;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetVacanciesRequest(
        int pageNumber,
        int pageSize,
        string accountLegalEntityPublicHashedId = "",
        string employerName = "",
        int? ukprn = null,
        string accountPublicHashedId = "",
        List<int> standardLarsCode = null,
        bool? nationwideOnly = null,
        double? lat = null,
        double? lon = null,
        uint? distanceInMiles = null,
        List<string> categories = null,
        uint? postedInLastNumberOfDays = null,
        List<string> additionalDataSources = null,
        string sort = "",
        bool? excludeNational = null)
        : IGetApiRequest
    {
        private readonly string _accountLegalEntityPublicHashedId = HttpUtility.UrlEncode(accountLegalEntityPublicHashedId);

        public string GetUrl => BuildGetUrl();
        public string Version => "2.0";

        private string BuildGetUrl()
        {
             var url = $"api/Vacancies?pageNumber={pageNumber}&pageSize={pageSize}";
            
             if (ukprn.HasValue)
             {
                 url += $"&ukprn={ukprn}";
             }
             if (!string.IsNullOrEmpty(_accountLegalEntityPublicHashedId))
             {
                 url += $"&accountLegalEntityPublicHashedId={_accountLegalEntityPublicHashedId}";
             }
             if (!string.IsNullOrEmpty(accountPublicHashedId))
             {
                 url += $"&accountPublicHashedId={accountPublicHashedId}";
             }
             if (standardLarsCode is { Count: >0 })
             {
                 url += $"&standardLarsCode={string.Join("&standardLarsCode=", standardLarsCode)}";
             }
             if (nationwideOnly.HasValue)
             {
                 url += $"&nationwideOnly={nationwideOnly}";
             }
             if (lat.HasValue)
             {
                 url += $"&lat={lat}";
             }
             if (lon.HasValue)
             {
                 url += $"&lon={lon}";
             }
             if (distanceInMiles.HasValue)
             {
                 url += $"&distanceInMiles={distanceInMiles}";
             }
             if (categories is { Count: >0 })
             {
                 url += $"&categories={string.Join("&categories=", categories)}";
             }
             if (!string.IsNullOrEmpty(sort))
             {
                 url += $"&sort={sort}";
             }
             if (postedInLastNumberOfDays.HasValue)
             {
                 url += $"&postedInLastNumberOfDays={postedInLastNumberOfDays}";
             }
             if (additionalDataSources is { Count: >0 })
             {
                 url += $"&additionalDataSources={string.Join("&additionalDataSources=", additionalDataSources)}";
             }
             if (excludeNational is not null)
             {
                 url += $"&excludeNational={excludeNational}";
             }
             if(!string.IsNullOrEmpty(employerName))
             {
                url += $"&employerName={HttpUtility.UrlEncode(employerName)}";
             }

             return url;
        }
    }
}
