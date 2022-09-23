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
        IBrowsingContext _browsingContext;

        public NationalAchievementRatesPageParser(ILogger<NationalAchievementRatesPageParser> logger, IBrowsingContext browsingContext)
        {
            _logger = logger;
            _browsingContext = browsingContext;
        }
        private const string ErrorMessage = "Error in finding the National Achievement Rates download page url";

        public async Task<string> GetCurrentDownloadFilePath(string nationalAchievementRatesDownloadPageUrl)
        {
            var yearTo = DateTime.Today.Year;
            var yearFrom = DateTime.Today.AddYears(-1).Year;
            IDocument document = null;
            var pageFound = false;
            while (!pageFound)
            {
                document = await _browsingContext.OpenAsync(string.Format(nationalAchievementRatesDownloadPageUrl, yearFrom, yearTo));
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