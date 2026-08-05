using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.FindApprenticeshipTraining.Services;

public interface ICachedStandardDetailsService
{
    Task<GetCourseLookupResponse> GetStandardDetails(string larsCode);
    Task<GetKsbsForCourseOptionResponse> GetKsbsForCourseOption(string larsCode);
}