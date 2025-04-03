using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class ApprenticeArticle
    {
        public string? EntryId { get; set; }
        public string? EntryTitle { get; set; }
        public bool? IsSaved { get; set; }
        public bool? LikeStatus { get; set; }
        public DateTime? SaveTime { get; set; }
    }

    public class ApprenticeArticleCollection
    {
        public List<ApprenticeArticle> ApprenticeArticles { get; set; }
    }
}