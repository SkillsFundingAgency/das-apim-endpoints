using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApimDeveloper.Api.AppStart
{
    public class DocumentationCacheInvalidator : IHostedService
    {
        private readonly ICacheStorageService _cacheStorageService;
        private readonly ILogger<DocumentationCacheInvalidator> _logger;

        public DocumentationCacheInvalidator(ICacheStorageService cacheStorageService, ILogger<DocumentationCacheInvalidator> logger)
        {
            _cacheStorageService = cacheStorageService;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var keys = await _cacheStorageService.GetCacheKeyRegistry("DocumentationKeys");

                if (keys.Any())
                {
                    _logger.LogInformation("Found {Count} documentation cache keys to delete.", keys.Count);
                }
                else
                {
                    _logger.LogInformation("No documentation cache keys found to delete.");
                }

                foreach (var key in keys)
                {
                    await _cacheStorageService.DeleteFromCache(key);
                    _logger.LogInformation("Deleted cache key: {Key}", key);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while invalidating documentation cache.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
