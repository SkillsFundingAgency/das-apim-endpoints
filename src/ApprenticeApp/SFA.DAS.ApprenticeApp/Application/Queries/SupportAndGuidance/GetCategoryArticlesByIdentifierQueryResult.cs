using System.Collections.Generic;
using Contentful.Core.Models;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.ApprenticeApp.Models.Contentful;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetCategoryArticlesByIdentifierQueryResult
    {
        public ContentfulCollection<Page> CategoryPage { get; set; }
        public List<ApprenticeAppArticlePage> Articles { get; set; }
        public ApprenticeArticleCollection ApprenticeArticles { get; set; }
    }
}