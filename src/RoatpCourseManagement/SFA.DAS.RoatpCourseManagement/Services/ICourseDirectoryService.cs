using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Services
{
    public interface ICourseDirectoryService
    {
        Task<string> GetAllProvidersData();
    }
}