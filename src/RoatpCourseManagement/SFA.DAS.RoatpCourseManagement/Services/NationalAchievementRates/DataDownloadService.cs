using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Services.NationalAchievementRates
{
    public class DataDownloadService : IDataDownloadService
    {
        private readonly HttpClient _client;

        public DataDownloadService(HttpClient client)
        {
            _client = client;
        }

        public async Task<Stream> GetFileStream(string downloadPath)
        {
            var response = await _client.GetAsync(downloadPath);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();

            return stream;
        }
    }
}