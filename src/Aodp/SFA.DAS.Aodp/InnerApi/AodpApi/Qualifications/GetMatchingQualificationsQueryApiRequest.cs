using SFA.DAS.Aodp.Application.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Specialized;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;

public class GetMatchingQualificationsQueryApiRequest : IGetApiRequest
{
    public string SearchTerm { get; set; }
    
    // Pagination
    public int? Skip { get; set; } = 0;
    public int? Take { get; set; } = 25;

    public string BaseUrl = "api/qualifications/GetMatchingQualifications";


    public GetMatchingQualificationsQueryApiRequest(string searchTerm, int? skip, int? take)
    {
        SearchTerm = searchTerm;
        Skip = skip;
        Take = take;
    }

    public string GetUrl
    {
        get
        {
            var queryParams = new NameValueCollection()
                {
                    { "SearchTerm", SearchTerm },
                };

            if (Skip.HasValue)
            {
                queryParams.Add("Skip", Skip.ToString());
            }

            if (Take.HasValue)
            {
                queryParams.Add("Take", Take.ToString());
            }

            var url = BaseUrl.AttachParameters(queryParams);

            return url;
        }
    }
}
