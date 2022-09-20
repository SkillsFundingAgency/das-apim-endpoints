using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.RoatpCourseManagement.Services.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Services
{
    public class CourseDirectoryService : ICourseDirectoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CourseDirectoryService> _logger;

        public CourseDirectoryService(HttpClient httpClient, ILogger<CourseDirectoryService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetAllProvidersData()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var response = await _httpClient.GetAsync("");
            _logger.LogInformation($"It took {stopWatch.Elapsed.TotalMilliseconds} milliseconds to get back a response from course directory");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"It took total {stopWatch.Elapsed.TotalMilliseconds} milliseconds to get the response and deserialise it");
            return jsonResponse;
        }
    }
}
