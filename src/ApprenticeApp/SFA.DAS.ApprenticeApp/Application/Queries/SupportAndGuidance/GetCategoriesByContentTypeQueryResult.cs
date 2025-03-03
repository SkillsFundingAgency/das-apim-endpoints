using Contentful.Core.Models;
using SFA.DAS.ApprenticeApp.Models.Contentful;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetCategoriesByContentTypeQueryResult
    {
        public ContentfulCollection<Page> CategoryPages { get; set; }
    }
}