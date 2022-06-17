using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.Services
{
    public interface ICourseDirectoryService
    {
        Task<string> GetAllProvidersData();
    }
}