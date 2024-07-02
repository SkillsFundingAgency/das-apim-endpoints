using Contentful.Core.Models;

namespace SFA.DAS.ApprenticeApp.Models.Contentful
{
    public class ApprenticeAppArticlePage : IEntity
    {
        public string Heading { get; set; }
        public string Slug { get; set; }
        public string Id { get; set; }
        public string Content { get; set; }
        public string ParentPageTitle { get; set; }
        public string ParentPageEntityId { get; set; }
        public SystemProperties Sys { get; set; }
    }
}