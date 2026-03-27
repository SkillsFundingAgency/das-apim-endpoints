using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.FindApprenticeshipTraining.Services;

public interface ICachedStandardDetailsService
{
    Task<StandardDetailsLookupResponse> GetStandardDetails(string larsCode);
    Task<GetKsbsForCourseOptionResponse> GetKsbsForCourseOption(string larsCode);
}