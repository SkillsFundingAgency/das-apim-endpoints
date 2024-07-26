using System.Collections.Generic;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.ApprenticeApp.Models.Contentful;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetUserSavedArticlesQueryResult
    {
        public List<Page> Articles { get; set; }
        public ApprenticeArticleCollection ApprenticeArticles { get; set; }
    }
}