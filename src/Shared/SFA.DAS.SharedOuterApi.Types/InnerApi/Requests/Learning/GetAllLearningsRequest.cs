using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Learning;

// Used by ApprenticeshipsManage and LearnerDataOuter
public class GetAllLearningsRequest(string ukprn, int academicYear, int page, int? pageSize) : IGetApiRequest
{
    public string GetUrl => $"/{ukprn}/academicyears/{academicYear}/learnings?page={page}&pageSize={pageSize}";
}