using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.NationalAchievementRatesLookup.Queries
{
    public class NationalAchievementRatesLookupQueryHandler : IRequestHandler<NationalAchievementRatesLookupQuery, NationalAchievementRatesLookupQueryResult>
    {
        private readonly INationalAchievementRatesLookupService _nationalAchievementRatesLookupService;
        private readonly ILogger<NationalAchievementRatesLookupQueryHandler> _logger;

        public NationalAchievementRatesLookupQueryHandler(INationalAchievementRatesLookupService nationalAchievementRatesLookupService, ILogger<NationalAchievementRatesLookupQueryHandler> logger)
        {
            _nationalAchievementRatesLookupService = locationLookupService;
            _logger = logger;
        }

        public async Task<NationalAchievementRatesLookupQueryResult> Handle(NationalAchievementRatesLookupQuery request, CancellationToken cancellationToken)
        {
            var result = await INationalAchievementRatesLookupService.GetNationalAchievementRates(request.ForYear);
            if (result == null)
            {
                _logger.LogWarning($"No National achievement rates found for year: {request.ForYear}.");
                return null;
            }
            _logger.LogInformation($"Found {result.Addresses.Count()} National achievement rates for year: {request.ForYear}");
            var response = new NationalAchievementRatesLookupQueryResult();
            response.NationalAchievementRates = result.Addresses.Select(a => (NationalAchievementRates)a).ToList();
            return response;
        }
    }
}
