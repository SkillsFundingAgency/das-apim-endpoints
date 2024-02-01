﻿using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData
{
    public class SearchOrganisationsRequest : IGetApiRequest
    {
        public string SearchTerm { get; }
        public int MaximumResults { get; set; }

        public SearchOrganisationsRequest(string searchTerm, int maximumResults)
        {
            SearchTerm = searchTerm;
            MaximumResults = maximumResults;
        }

        public string GetUrl => $"?searchTerm={HttpUtility.UrlEncode(SearchTerm)}&maximumResults={MaximumResults}";
    }
}
