using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Services.NationalAchievementRates
{
    public class NationalAchievementRatesPageParser : INationalAchievementRatesPageParser
    {
        private readonly ILogger<NationalAchievementRatesPageParser> _logger;
        private readonly HttpClient _client;
      

        public NationalAchievementRatesPageParser(ILogger<NationalAchievementRatesPageParser> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;
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
                var response = await _client.GetAsync(string.Format(nationalAchievementRatesDownloadPageUrl, yearFrom, yearTo));
                if (response.StatusCode != HttpStatusCode.NotFound)
                {
                    pageFound = true;
                    var parser = new HtmlParser();
                    document = parser.ParseDocument(await response.Content.ReadAsStringAsync());
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