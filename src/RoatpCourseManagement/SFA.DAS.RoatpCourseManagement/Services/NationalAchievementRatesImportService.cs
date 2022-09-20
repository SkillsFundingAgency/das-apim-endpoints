using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Services.Interfaces;
using SFA.DAS.RoatpCourseManagement.Services.Models;
using SFA.DAS.RoatpCourseManagement.Services.Models.ImportTypes;

namespace SFA.DAS.RoatpCourseManagement.Services
{
    public class NationalAchievementRatesImportService : INationalAchievementRatesImportService
    {
        private readonly INationalAchievementRatesPageParser _pageParser;
        private readonly IImportAuditRepository _auditRepository;
        private readonly IDataDownloadService _downloadService;
        private readonly IZipArchiveHelper _zipArchiveHelper;
        private readonly INationalAchievementRateImportRepository _importRepository;
        private readonly INationalAchievementRateRepository _repository;
        private readonly ILogger<NationalAchievementRatesImportService> _logger;

        public NationalAchievementRatesImportService (
            INationalAchievementRatesPageParser pageParser,
            IImportAuditRepository auditRepository,
            IDataDownloadService downloadService,
            IZipArchiveHelper zipArchiveHelper,
            INationalAchievementRateImportRepository importRepository,
            INationalAchievementRateRepository repository,
            ILogger<NationalAchievementRatesImportService> logger)
        {
            _pageParser = pageParser;
            _auditRepository = auditRepository;
            _downloadService = downloadService;
            _zipArchiveHelper = zipArchiveHelper;
            _importRepository = importRepository;
            _repository = repository;
            _logger = logger;
        }

        public async Task ImportData()
        {
            var timeStarted = DateTime.UtcNow;
            var downloadFilePathTask = _pageParser.GetCurrentDownloadFilePath();
            var auditTask = _auditRepository.GetLastImportByType(ImportType.NationalAchievementRates);

            await Task.WhenAll(downloadFilePathTask, auditTask);

            if (auditTask.Result != null && auditTask.Result.FileName.Equals(downloadFilePathTask.Result,
                StringComparison.CurrentCultureIgnoreCase))
            {
                _logger.LogInformation("No new achievement rate data to load");
                return;
            }

            var dataFile = await _downloadService.GetFileStream(downloadFilePathTask.Result);

            var data = _zipArchiveHelper.ExtractModelFromCsvFileZipStream<NationalAchievementRateCsv>(dataFile, Constants.NationalAchievementRatesCsvFileName);

            _logger.LogInformation("Clearing import table");
            _importRepository.DeleteAll();

            _logger.LogInformation("Loading to import table");
            await _importRepository.InsertMany(data.Select(c => (NationalAchievementRateImport) c).ToList());    
            
            _logger.LogInformation("Clearing main table");
            _repository.DeleteAll();

            _logger.LogInformation("Importing to main table");
            var items = (await _importRepository.GetAllWithAchievementData()).ToList();

            await _repository.InsertMany(items.Select(c => (NationalAchievementRate) c).ToList());

            await _auditRepository.Insert(new ImportAudit(timeStarted, items.Count, ImportType.NationalAchievementRates,
                downloadFilePathTask.Result));
            
            _logger.LogInformation("Import Complete");
        }
    }
}