using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ICourseService
    {
        Task<GetRoutesListResponse> GetRoutes();
        Task<GetCourseLevelsListResponse> GetLevels();
        Task<T> GetActiveStandards<T>(string cacheItemName);
    }
}