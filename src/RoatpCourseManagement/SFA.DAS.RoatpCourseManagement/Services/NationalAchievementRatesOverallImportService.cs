using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Services.Interfaces;
using SFA.DAS.RoatpCourseManagement.Services.Models;
using SFA.DAS.RoatpCourseManagement.Services.Models.ImportTypes;

namespace SFA.DAS.RoatpCourseManagement.Services
{
    public class NationalAchievementRatesOverallImportService : INationalAchievementRatesOverallImportService
    {
        private readonly INationalAchievementRatesPageParser _pageParser;
        private readonly IImportAuditRepository _auditRepository;
        private readonly IDataDownloadService _downloadService;
        private readonly IZipArchiveHelper _zipArchiveHelper;
        private readonly INationalAchievementRateOverallImportRepository _importRepository;
        private readonly INationalAchievementRateOverallRepository _repository;
        private readonly ILogger<NationalAchievementRatesOverallImportService> _logger;

        public NationalAchievementRatesOverallImportService (INationalAchievementRatesPageParser pageParser,
            IImportAuditRepository auditRepository,
            IDataDownloadService downloadService,
            IZipArchiveHelper zipArchiveHelper,
            INationalAchievementRateOverallImportRepository importRepository,
            INationalAchievementRateOverallRepository repository,
            ILogger<NationalAchievementRatesOverallImportService> logger)
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
            var auditTask = _auditRepository.GetLastImportByType(ImportType.NationalAchievementRatesOverall);

            await Task.WhenAll(downloadFilePathTask, auditTask);

            if (auditTask.Result != null && auditTask.Result.FileName.Equals(downloadFilePathTask.Result,
                StringComparison.CurrentCultureIgnoreCase))
            {
                _logger.LogInformation("No new overall achievement rate data to load");
                return;
            }

            var dataFile = await _downloadService.GetFileStream(downloadFilePathTask.Result);

            var data = _zipArchiveHelper.ExtractModelFromCsvFileZipStream<NationalAchievementRateOverallCsv>(dataFile, Constants.NationalAchievementRatesOverallCsvFileName);

            _logger.LogInformation("Clearing import table");
            _importRepository.DeleteAll();

            _logger.LogInformation("Loading to import table");
            await _importRepository.InsertMany(data
                .Where(c=>
                    !c.SectorSubjectArea.Contains("All Sector Subject Area"))
                .Where(c=>c.InstitutionType == "All Institution Type")
                .Where(c=>c.Age == "All Age")
                .Select(c => (NationalAchievementRateOverallImport) c)
                
                .ToList());    
            
            _logger.LogInformation("Clearing main table");
            _repository.DeleteAll();

            _logger.LogInformation("Importing to main table");
            var items = (await _importRepository.GetAllWithAchievementData()).ToList();

            await _repository.InsertMany(items.Select(c => (NationalAchievementRateOverall) c).ToList());

            await _auditRepository.Insert(new ImportAudit(timeStarted, items.Count, ImportType.NationalAchievementRatesOverall,
                downloadFilePathTask.Result));
            
            _logger.LogInformation("Import Complete");
        }
    }
}