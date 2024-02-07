using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData
{
    public class GetPublicSectorOrganisationsRequest : IGetPagedApiRequest
    {
        public readonly string SearchTerm;
        public int PageNumber { get; }
        public int PageSize { get; }

        public GetPublicSectorOrganisationsRequest(string searchTerm, int pageSize, int pageNumber)
        {
            SearchTerm = searchTerm;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }

        public string GetPagedUrl => $"publicsectorbodies?searchTerm={HttpUtility.UrlEncode(SearchTerm)}&pageSize={PageSize}&pageNumber={PageNumber}";
    }
}