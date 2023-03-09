using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Services
{
    public interface ICachedCoursesService
    {
        Task<GetStandardsListResponse> GetCourses();
    }
}