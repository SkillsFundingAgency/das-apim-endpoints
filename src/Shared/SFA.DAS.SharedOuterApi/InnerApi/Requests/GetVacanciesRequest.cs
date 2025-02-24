﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetVacanciesRequest : IGetApiRequest
    {
        private readonly int _pageNumber;
        private readonly int _pageSize;
        private readonly string _accountLegalEntityPublicHashedId;
        private readonly int? _ukprn;
        private readonly string _accountPublicHashedId;
        private readonly List<int> _standardLarsCode;
        private readonly bool? _nationwideOnly;
        private readonly double? _lat;
        private readonly double? _lon;
        private readonly uint? _distanceInMiles;
        private readonly List<string> _categories;
        private readonly uint? _postedInLastNumberOfDays;
        private readonly string _sort;
        private readonly List<string> _additionalDataSources;

        public GetVacanciesRequest(int pageNumber,
            int pageSize,
            string accountLegalEntityPublicHashedId = "",
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
            string sort = "")
        {
            _pageNumber = pageNumber;
            _pageSize = pageSize;
            _accountLegalEntityPublicHashedId = HttpUtility.UrlEncode(accountLegalEntityPublicHashedId);
            _ukprn = ukprn;
            _accountPublicHashedId = accountPublicHashedId;
            _standardLarsCode = standardLarsCode;
            _nationwideOnly = nationwideOnly;
            _lat = lat;
            _lon = lon;
            _distanceInMiles = distanceInMiles;
            _categories = categories;
            _postedInLastNumberOfDays = postedInLastNumberOfDays;
            _additionalDataSources = additionalDataSources;
            _sort = sort;
        }

        public string GetUrl => BuildGetUrl();
        public string Version => "2.0";

        private string BuildGetUrl()
        {
            var url = $"api/Vacancies?pageNumber={_pageNumber}&pageSize={_pageSize}";

            if (_ukprn.HasValue)
            {
                url += $"&ukprn={_ukprn}";
            }
            if (!string.IsNullOrEmpty(_accountLegalEntityPublicHashedId))
            {
                url += $"&accountLegalEntityPublicHashedId={_accountLegalEntityPublicHashedId}";
            }
            if (!string.IsNullOrEmpty(_accountPublicHashedId))
            {
                url += $"&accountPublicHashedId={_accountPublicHashedId}";
            }
            if (_standardLarsCode != null && _standardLarsCode.Any())
            {
                url += $"&standardLarsCode={string.Join("&standardLarsCode=", _standardLarsCode)}";
            }
            if (_nationwideOnly.HasValue)
            {
                url += $"&nationwideOnly={_nationwideOnly}";
            }
            if (_lat.HasValue)
            {
                url += $"&lat={_lat}";
            }
            if (_lon.HasValue)
            {
                url += $"&lon={_lon}";
            }
            if (_distanceInMiles.HasValue)
            {
                url += $"&distanceInMiles={_distanceInMiles}";
            }
            if (_categories != null && _categories.Any())
            {
                url += $"&categories={string.Join("&categories=", _categories)}";
            }
            if (!string.IsNullOrEmpty(_sort))
            {
                url += $"&sort={_sort}";
            }
            if (_postedInLastNumberOfDays.HasValue)
            {
                url += $"&postedInLastNumberOfDays={_postedInLastNumberOfDays}";
            }
            if (_additionalDataSources != null && _additionalDataSources.Any())
            {
                url += $"&additionalDataSources={string.Join("&additionalDataSources=", _additionalDataSources)}";
            }

            return url;
        }
    }
}
