using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SFA.DAS.LearnerData.Responses;

namespace SFA.DAS.LearnerData.Extensions;

public static class HttpContextExtensions
{
    /// <summary>
    /// Adds 'Links' to response headers. 
    /// </summary>
    public static void SetPageLinksInResponseHeaders<T>(this HttpContext httpContext, PagedQuery request, PagedQueryResult<T> response)
    {
        var httpRequest = httpContext.Request;

        var baseUrl = GetBaseUrlFrom(httpRequest);

        var links = new List<string>();

        if (NotFirstPage(response))
        {
            var previousPage = request.Page - 1;
            var prevLink = QueryHelpers.AddQueryString(baseUrl, "page", previousPage.ToString());
            prevLink = QueryHelpers.AddQueryString(prevLink, "pageSize", response.PageSize.ToString());
            links.Add($"{prevLink};rel=\"prev\"");
        }

        if (NotLastPage(request, response))
        {
            var nextPage = request.Page + 1;
            var nextLink = QueryHelpers.AddQueryString(baseUrl, "page", nextPage.ToString());
            nextLink = QueryHelpers.AddQueryString(nextLink, "pageSize", response.PageSize.ToString());

            links.Add($"{nextLink};rel=\"next\"");
        }

        var pageLinks = new KeyValuePair<string, StringValues>("links", string.Join(",", links));
        httpContext.Response.Headers.Add(pageLinks);
    }

    private static string GetBaseUrlFrom(HttpRequest httpRequest)
    {
        var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.Path}";

        foreach (var value in httpRequest.Query)
        {
            if (!value.Key.Equals("page", StringComparison.OrdinalIgnoreCase) && !value.Key.Equals("pageSize", StringComparison.OrdinalIgnoreCase))
            {
                baseUrl = QueryHelpers.AddQueryString(baseUrl, value.Key, value.Value.ToString());
            }
        }

        return baseUrl;
    }

    private static bool NotLastPage<T>(PagedQuery request, PagedQueryResult<T> response)
    {
        return request.Offset < response.TotalItems - response.PageSize;
    }

    private static bool NotFirstPage<T>(PagedQueryResult<T> response)
    {
        return response.Page > 1;
    }
}
