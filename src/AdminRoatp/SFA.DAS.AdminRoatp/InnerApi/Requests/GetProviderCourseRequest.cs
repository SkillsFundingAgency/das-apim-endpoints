using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public record GetProviderCourseRequest(int Ukprn, string LarsCode) : IGetApiRequest
{
    public string GetUrl => $"providers/{Ukprn}/courses/{LarsCode}";
}
