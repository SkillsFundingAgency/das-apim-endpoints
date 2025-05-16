using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetLearnersForProviderRequest(long providerId, int academicYear, string filter, string sortColumn, bool sortDescending, int page) : IGetApiRequest
    {
        public string GetUrl => $"providers/{providerId}/learners?academicYear={academicYear}&filter={HttpUtility.UrlEncode(filter)}&sortColumn={sortColumn}&sortDescending={sortDescending}&page={page}";
    }
}