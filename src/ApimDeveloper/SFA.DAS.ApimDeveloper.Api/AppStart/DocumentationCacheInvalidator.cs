using Microsoft.Extensions.Hosting;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApimDeveloper.Api.AppStart
{
    public class DocumentationCacheInvalidator : IHostedService
    {
        private readonly ICacheStorageService _cacheStorageService;

        public DocumentationCacheInvalidator(ICacheStorageService cacheStorageService)
        {
            _cacheStorageService = cacheStorageService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var keys = await _cacheStorageService.GetCacheKeyRegistry("DocumentationKeys");
            foreach (var key in keys)
            {
                await _cacheStorageService.DeleteFromCache(key);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
