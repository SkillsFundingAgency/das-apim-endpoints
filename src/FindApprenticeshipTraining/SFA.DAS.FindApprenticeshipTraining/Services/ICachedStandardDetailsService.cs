using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.FindApprenticeshipTraining.Services;

public interface ICachedStandardDetailsService
{
    Task<StandardDetailResponse> GetStandardDetails(string larsCode);
}