using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCoursesService;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IProviderCoursesService
    {
        public Task<List<ProviderCourse>> GetProviderCourses(long trainingProviderId);
    }
}
