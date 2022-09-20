using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Services.Interfaces
{
    public interface ICourseDirectoryService
    {
        Task<string> GetAllProvidersData();
    }
}