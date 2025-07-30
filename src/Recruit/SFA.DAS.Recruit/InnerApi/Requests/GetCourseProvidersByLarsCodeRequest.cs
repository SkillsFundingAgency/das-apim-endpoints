using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public record GetCourseProvidersByLarsCodeRequest(int LarsCode, int Page) : IGetApiRequest
{
    public string GetUrl => $"api/courses/{LarsCode}/providers?orderBy=EmployerProviderRating&pageSize=500&page={Page}";
}