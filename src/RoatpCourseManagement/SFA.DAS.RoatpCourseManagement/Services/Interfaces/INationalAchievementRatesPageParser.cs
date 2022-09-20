using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Services.Interfaces
{
    public interface INationalAchievementRatesPageParser
    {
        Task<string> GetCurrentDownloadFilePath();
    }
}