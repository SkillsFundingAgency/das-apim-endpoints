using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public record GetProviderCourseTypesRequest(int Ukprn) : IGetApiRequest
{
    public string GetUrl => $"providers/{Ukprn}/course-types";
}
