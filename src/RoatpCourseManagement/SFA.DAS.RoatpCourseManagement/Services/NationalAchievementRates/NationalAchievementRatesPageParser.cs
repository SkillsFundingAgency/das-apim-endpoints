using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.RoatpCourseManagement.Services.NationalAchievementRates
{
    public class NationalAchievementRatesPageParser : INationalAchievementRatesPageParser
    {
        private readonly ILogger<NationalAchievementRatesPageParser> _logger;

        public NationalAchievementRatesPageParser(ILogger<NationalAchievementRatesPageParser> logger)
        {
            _logger = logger;
        }

        private const string NationalAchievementRatesPageUrl = "https://www.gov.uk/government/statistics/national-achievement-rates-tables-{0}-to-{1}";
        private const string ErrorMessage = "Error in finding the National Achievement Rates download page url";
        public async Task<string> GetCurrentDownloadFilePath()
        {
            var yearTo = DateTime.Today.Year;
            var yearFrom = DateTime.Today.AddYears(-1).Year;
            var config = AngleSharp.Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            IDocument document = null;
            var pageFound = false;
            while (!pageFound)
            {
                document = await context.OpenAsync(string.Format(NationalAchievementRatesPageUrl, yearFrom, yearTo));
                if (document.StatusCode != HttpStatusCode.NotFound)
                {
                    pageFound = true;
                }
                else
                {
                    yearTo--;
                    yearFrom--;
                }
                if (yearTo < DateTime.Today.AddYears(-10).Year)
                {
                    _logger.LogError(ErrorMessage);
                    throw new InvalidOperationException(ErrorMessage);
                }
            }

            var downloadHref = document
                .QuerySelectorAll($"a:contains('{yearFrom} to {yearTo} apprenticeship NARTs overall CSV')")
                .First()
                .GetAttribute("Href");

            var uri = new Uri(downloadHref);

            return uri.AbsoluteUri;
        }
    }
}