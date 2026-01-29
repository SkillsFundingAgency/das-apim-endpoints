using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindApprenticeshipTraining.Services;

public class CachedCoursesService : ICachedCoursesService
{
    private const int CacheDurationInHours = 1;

    private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
    private readonly ICacheStorageService _cacheStorageService;

    public CachedCoursesService(
        ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
        ICacheStorageService cacheStorageService)
    {
        _coursesApiClient = coursesApiClient;
        _cacheStorageService = cacheStorageService;
    }

    public async Task<GetStandardsListResponse> GetCourses()
    {
        var courses = await _cacheStorageService
            .RetrieveFromCache<GetStandardsListResponse>(nameof(GetStandardsListResponse));

        if (courses != null)
            return courses;

        courses = await _coursesApiClient.Get<GetStandardsListResponse>(new GetActiveStandardsListRequest());
        await _cacheStorageService.SaveToCache(nameof(GetStandardsListResponse), courses, CacheDurationInHours);

        return courses;
    }

    public async Task<GetStandardsListItem> GetCourseDetails(int larsCode)
    {
        var course = await _cacheStorageService.RetrieveFromCache<GetStandardsListItem>(larsCode.ToString());

        if (course != null) return course;

        ApiResponse<GetStandardsListItem> standardResponse = await _coursesApiClient.GetWithResponseCode<GetStandardsListItem>(new GetStandardRequest(larsCode));

        standardResponse.EnsureSuccessStatusCode();

        await _cacheStorageService.SaveToCache(larsCode.ToString(), standardResponse.Body, CacheDurationInHours);

        return standardResponse.Body;
    }
}
