using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Configuration;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.Extensions.Configuration;
using SFA.DAS.ApprenticeApp.Models.Contentful;

namespace SFA.DAS.ApprenticeApp.Client
{
    public class ContentClient : IContentClient
    {
        private readonly IContentfulClient _contentfulClient;
        private readonly IConfiguration _configuration;

        public ContentClient(
            IContentTypeResolver contentTypeResolver,
            IContentfulClient contentfulClient,
            IConfiguration configuration)
        {
            _contentfulClient = contentfulClient;
            _configuration = configuration;
            _contentfulClient.ContentTypeResolver = contentTypeResolver;
        }

        public async Task<List<T>> GetEntries<T>(string contentType, string field, string value) where T : IEntity
        {
            var queryBuilder = QueryBuilder<T>.New.ContentTypeIs(contentType).FieldEquals(field, value);

            var items = await _contentfulClient.GetEntries(queryBuilder);

            return items?.Items?.ToList();
        }

        public async Task<ContentfulCollection<Page>> GetAllPages()
        {
            var items = await _contentfulClient.GetEntries<Page>();
            return items;
        }

        public async Task<Page> GetByEntryId(string id)
        {
            var item = await _contentfulClient.GetEntry<Page>(id);
            return item;
        }

        public async Task<ContentfulCollection<Page>> GetByEntryIdWithChildren(string id)
        {
            var builder = new QueryBuilder<Page>().FieldEquals(f => f.Sys.Id, id).Include(2);
            var entries = await _contentfulClient.GetEntries(builder);

            return entries;
        }

        public async Task<ContentfulCollection<Page>> GetBySlugWithChildren(string slug)
        {
            var builder = new QueryBuilder<Page>().ContentTypeIs("apprenticeAppCategory").FieldEquals(f => f.Slug, slug).Include(2);
            var entries = await _contentfulClient.GetEntries(builder);

            return entries;
        }

        public async Task<ContentfulCollection<T>> GetPagesByContentType<T>(string contentType) where T : IEntity
        {
            var entries = await _contentfulClient.GetEntriesByType<T>(contentType);
            return entries;
        }

    }
}