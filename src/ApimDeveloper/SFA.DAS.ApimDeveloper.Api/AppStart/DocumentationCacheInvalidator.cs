using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApimDeveloper.Api.AppStart
{
    public class DocumentationCacheInvalidator
    {
        private readonly ICacheStorageService _cacheStorageService;
        private readonly ILogger<DocumentationCacheInvalidator> _logger;

        public DocumentationCacheInvalidator(ICacheStorageService cacheStorageService, ILogger<DocumentationCacheInvalidator> logger)
        {
            _cacheStorageService = cacheStorageService;
            _logger = logger;
        }

        public async Task InvalidateAsync()
        {
            try
            {
                var keys = await _cacheStorageService.GetCacheKeyRegistry("DocumentationKeys");

                foreach (var key in keys)
                {
                    await _cacheStorageService.DeleteFromCache(key);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while invalidating documentation cache.");
            }
        }
    }
}
