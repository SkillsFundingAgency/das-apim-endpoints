using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Globalization;
using System.Web;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetLearnersForProviderRequest(long providerId, string filter, string sortColumn, bool sortDescending, int page, int? pagesize, int? startMonth, int startYear, DateTime? maxStartDate, string excludeUlns, int? courseCode) : IGetApiRequest
    {
        public string GetUrl => $"providers/{providerId}/learners?filter={HttpUtility.UrlEncode(filter)}&sortColumn={sortColumn}&sortDescending={sortDescending}&page={page}&pageSize={pagesize}&startMonth={startMonth}&startYear={startYear}&maxStartDate={maxStartDate?.ToString(CultureInfo.InvariantCulture)}&excludeUlns={excludeUlns}&coursecode={courseCode}";
    }
}