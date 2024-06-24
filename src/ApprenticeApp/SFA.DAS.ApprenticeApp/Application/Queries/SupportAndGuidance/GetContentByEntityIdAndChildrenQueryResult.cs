using System.Collections.Generic;
using Contentful.Core.Models;
using SFA.DAS.ApprenticeApp.Models.Contentful;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetContentByEntityIdAndChildrenQueryResult
    {
        public ContentfulCollection<Page> Parent { get; set; }
        public List<ApprenticeAppArticlePage> ChildArticles { get; set; }
    }
}