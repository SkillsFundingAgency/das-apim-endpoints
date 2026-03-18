using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;

namespace SFA.DAS.SharedOuterApi.Types.Interfaces
{
    public interface ICourseService
    {
        Task<GetRoutesListResponse> GetRoutes();
        Task<GetCourseLevelsListResponse> GetLevels();
        Task<T> GetActiveStandards<T>(string cacheItemName);
    }
}