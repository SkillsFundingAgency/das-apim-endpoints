using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Search;

namespace SFA.DAS.Campaign.Contentful
{
    public class ContentfulService : IContentfulService
    {
        private readonly IContentfulClient _client;

        public ContentfulService(IContentfulClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<T> GetEntryForAsync<T>(string entryId, QueryBuilder<T> queryBuilder, CancellationToken cancellationToken = default) where T : IContentType
        {
            return await _client.GetEntry(entryId, queryBuilder, cancellationToken);
        }

        public async Task<Article> GetArticleForAsync(string entryId, CancellationToken cancellationToken = default)
        {
            var queryBuilder = QueryBuilder<Article>.New.ContentTypeIs(ContentfulConstants.ArticleContentTypeId)
                .FieldEquals(ContentfulConstants.ContentTypeSystemIdPath, entryId);

            var articles = await _client.GetEntries<Article>(queryBuilder, cancellationToken);

            return articles.FirstOrDefault();
        }

        public async Task<Article> GetArticleForAsync(string hubType, string slug, CancellationToken cancellationToken = default)
        {
            var queryBuilder = QueryBuilder<Article>.New.ContentTypeIs(ContentfulConstants.ArticleContentTypeId)
                .FieldEquals(ContentfulConstants.HubTypePath, hubType).FieldEquals(ContentfulConstants.SlugPath, slug);

            var articles = await _client.GetEntries<Article>(queryBuilder, cancellationToken);

            return articles.FirstOrDefault();
        }
    }
}
