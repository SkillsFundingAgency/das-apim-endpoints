using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Services.NationalAchievementRates
{
    public interface INationalAchievementRatesPageParser
    {
        Task<string> GetCurrentDownloadFilePath();
    }
}