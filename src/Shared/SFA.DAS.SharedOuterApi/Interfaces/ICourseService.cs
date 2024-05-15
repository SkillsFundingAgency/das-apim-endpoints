using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ICourseService
    {
        Task<GetRoutesListResponse> GetRoutes();
        Task<GetCourseLevelsListResponse> GetLevels();
        Task<T> GetActiveStandards<T>(string cacheItemName);
    }
}