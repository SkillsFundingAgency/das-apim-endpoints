using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Interfaces
{
    public interface ICachedCoursesService
    {
        Task<GetStandardsListResponse> GetCourses();
    }
}