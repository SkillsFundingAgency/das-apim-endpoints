using System;
using System.Threading.Tasks;
using Contentful.Core.Models;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeApp.Client;
using SFA.DAS.ApprenticeApp.Models.Contentful;

namespace SFA.DAS.ApprenticeApp.Services
{
    public class ContentService : IContentService
    {
        private readonly ILogger<ContentService> _logger;
        private readonly IContentClient _contentClient;

        public ContentService(
            ILogger<ContentService> logger,
            IContentClient contentClient
            )
        {
            _logger = logger;
            _contentClient = contentClient;
        }

        public async Task<ContentfulCollection<Page>> GetCategoriesByContentType<T>(string contentType)
        {
            try
            {
                _logger.LogInformation($"Getting contentful GetPagesByContentType {contentType}");

                var items = await _contentClient.GetPagesByContentType<Page>(contentType);

                if (items != null)
                {
                    return items;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to process contentful GetPagesByContentType");
                throw;
            }
        }

        public async Task<Page> GetPageById(string id)
        {
            try
            {
                _logger.LogInformation($"Getting contentful GetPageById {id}");

                var item = await _contentClient.GetByEntryId(id);

                if (item != null)
                {
                    return item;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to process contentful GetPageById");
                throw;
            }
        }

        public async Task<ContentfulCollection<Page>> GetPageByIdWithChildren(string id)
        {
            try
            {
                _logger.LogInformation($"Getting contentful GetPageByIdWithChildren {id}");

                var items = await _contentClient.GetByEntryIdWithChildren(id);

                if (items != null)
                {
                    return items;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to process contentful GetPageByIdWithChildren");
                throw;
            }
        }

        public async Task<ContentfulCollection<Page>> GetCategoryArticlesByIdentifier(string slug)
        {
            try
            {
                _logger.LogInformation($"Getting contentful GetPageBySlugWithChildren {slug}");

                var items = await _contentClient.GetBySlugWithChildren(slug);

                if (items != null)
                {
                    return items;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to process contentful GetPageBySlugWithChildren");
                throw;
            }
        }
    }
}