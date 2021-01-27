using System.Threading.Tasks;
using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Interfaces
{
    public interface ICachedCoursesService
    {
        Task<GetStandardsListResponse> GetCourses();
    }
}