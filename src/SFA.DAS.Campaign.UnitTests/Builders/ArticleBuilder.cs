using Contentful.Core.Models;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.UnitTests.Builders
{
    public class ArticleBuilder
    {
        private readonly Article _article;

        public ArticleBuilder()
        {
            _article = new Article();
        }

        public ArticleBuilder SetTitle(string title)
        {
            _article.Title = title;

            return this;
        }

        public ArticleBuilder SetHubType(string hubType)
        {
            _article.HubType = hubType;

            return this;
        }

        public ArticleBuilder SetSlug(string slug)
        {
            _article.Slug = slug;

            return this;
        }

        public ArticleBuilder SetId(string id)
        {
            _article.Sys = new SystemProperties
            {
                Id = id
            };

            return this;
        }

        public static ArticleBuilder New()
        {
            return new ArticleBuilder();
        }

        public Article Build()
        {
            return _article;
        }
    }
}
