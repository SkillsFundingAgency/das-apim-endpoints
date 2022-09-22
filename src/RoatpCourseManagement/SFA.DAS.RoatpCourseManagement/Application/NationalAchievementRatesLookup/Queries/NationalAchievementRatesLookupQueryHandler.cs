using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.RoatpCourseManagement.Services.Models.ImportTypes;
using SFA.DAS.RoatpCourseManagement.Services.Models;
using SFA.DAS.RoatpCourseManagement.Services.NationalAchievementRates;

namespace SFA.DAS.RoatpCourseManagement.Application.NationalAchievementRatesLookup.Queries
{
    public class NationalAchievementRatesLookupQueryHandler : IRequestHandler<NationalAchievementRatesLookupQuery, NationalAchievementRatesLookupQueryResult>
    {
        private readonly ILogger<NationalAchievementRatesLookupQueryHandler> _logger;
        private readonly INationalAchievementRatesPageParser _pageParser;
        private readonly IDataDownloadService _downloadService;
        private readonly IZipArchiveHelper _zipArchiveHelper;
        private const string NationalAchievementRatesCsvFileName = "NART_vDM_Apprenticeships_Institution_SectorSubjectArea_Overall.csv";
        private const string NationalAchievementRatesOverallCsvFileName = "NART_vDM_Apprenticeships_SectorSubjectArea_Overall.csv";


        public NationalAchievementRatesLookupQueryHandler(INationalAchievementRatesPageParser pageParser,
            IDataDownloadService downloadService,
            IZipArchiveHelper zipArchiveHelper,
            ILogger<NationalAchievementRatesLookupQueryHandler> logger)
        {
            _pageParser = pageParser;
            _downloadService = downloadService;
            _zipArchiveHelper = zipArchiveHelper;
            _logger = logger;
        }

        public async Task<NationalAchievementRatesLookupQueryResult> Handle(NationalAchievementRatesLookupQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start getting National achievement rates and Overall National achievement rates.");

            var downloadFilePath = await _pageParser.GetCurrentDownloadFilePath();

            var dataFile = await _downloadService.GetFileStream(downloadFilePath);

            var response = new NationalAchievementRatesLookupQueryResult();

            var dataOverallAchievementRates = _zipArchiveHelper.ExtractModelFromCsvFileZipStream<NationalAchievementRateOverallCsv>(dataFile, NationalAchievementRatesOverallCsvFileName);
            response.OverallAchievementRates = dataOverallAchievementRates
                .Where(c => !c.SectorSubjectArea.Contains("All Sector Subject Area"))
                .Where(c => c.InstitutionType == "All Institution Type")
                .Where(c => c.Age == "All Age")
                .Select(c => (NationalAchievementRateOverall)c).ToList();

            if (!response.OverallAchievementRates.Any())
            {
                _logger.LogWarning($"No Overall National achievement rates found");
            }

            var dataNationalAchievementRate = _zipArchiveHelper.ExtractModelFromCsvFileZipStream<NationalAchievementRateCsv>(dataFile, NationalAchievementRatesCsvFileName);
            response.NationalAchievementRates = dataNationalAchievementRate.Select(c => (NationalAchievementRate)c).ToList();
            
            if (!response.NationalAchievementRates.Any())
            {
                _logger.LogWarning($"No National achievement rates found.");
            }
            _logger.LogInformation($"Found {response.NationalAchievementRates.Count} National achievement rates and {response.OverallAchievementRates.Count} Overall National achievement rates.");
            return response;
        }
    }
}
