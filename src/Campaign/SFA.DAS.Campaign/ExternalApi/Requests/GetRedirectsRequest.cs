using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Campaign.ExternalApi.Requests
{
    public class GetRedirectsRequest : IGetApiRequest
    {
        /// <summary>
        /// Contentful returns 100 entries by default and caps a single page at 1000, which is far more redirects
        /// than the site is ever expected to carry. Paging would be needed beyond that.
        /// </summary>
        public const int MaxRedirects = 1000;

        public string GetUrl => $"entries?content_type={ContentfulConstants.RedirectContentTypeId}&limit={MaxRedirects}";
    }
}
