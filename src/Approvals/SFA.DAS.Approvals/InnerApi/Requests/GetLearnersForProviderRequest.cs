﻿using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetLearnersForProviderRequest(long providerId, string filter, string sortColumn, bool sortDescending, int page, int? pagesize, int? startMonth, int startYear, string excludeUlns) : IGetApiRequest
    {
        public string GetUrl => $"providers/{providerId}/learners?filter={HttpUtility.UrlEncode(filter)}&sortColumn={sortColumn}&sortDescending={sortDescending}&page={page}&pageSize={pagesize}&startMonth={startMonth}&startYear={startYear}&excludeUlns={excludeUlns}";
    }
}