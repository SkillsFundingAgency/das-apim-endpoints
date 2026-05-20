using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ProviderCoursesService;

namespace SFA.DAS.SharedOuterApi.Types.Interfaces
{
    public interface IProviderCoursesService
    {
        public Task<List<ProviderCourse>> GetProviderCourses(long trainingProviderId);
    }
}
