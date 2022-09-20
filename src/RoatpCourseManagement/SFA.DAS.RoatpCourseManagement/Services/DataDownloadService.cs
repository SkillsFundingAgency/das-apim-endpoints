using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.RoatpCourseManagement.Services.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Services
{
    public class DataDownloadService : IDataDownloadService
    {
        private readonly HttpClient _client;

        public DataDownloadService (HttpClient client)
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