using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public record GetCourseProvidersByLarsCodeRequest(int LarsCode, int Page) : IGetApiRequest
{
    public string GetUrl => $"api/courses/{LarsCode}/providers?orderBy=EmployerProviderRating&pageSize=500&page={Page}";
}