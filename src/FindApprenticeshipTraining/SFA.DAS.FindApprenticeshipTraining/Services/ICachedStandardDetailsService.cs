using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;

namespace SFA.DAS.FindApprenticeshipTraining.Services;

public interface ICachedStandardDetailsService
{
    Task<StandardDetailResponse> GetStandardDetails(string larsCode);
}