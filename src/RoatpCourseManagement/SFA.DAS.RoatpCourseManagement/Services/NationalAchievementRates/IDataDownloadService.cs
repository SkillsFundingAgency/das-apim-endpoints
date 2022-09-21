using System.IO;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Services.NationalAchievementRates
{
    public interface IDataDownloadService
    {
        Task<Stream> GetFileStream(string downloadPath);
    }
}