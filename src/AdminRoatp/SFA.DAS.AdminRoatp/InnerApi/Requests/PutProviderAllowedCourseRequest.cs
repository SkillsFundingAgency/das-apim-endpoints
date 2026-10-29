using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public record PutProviderAllowedCourseRequest : IPutApiRequest<PutProviderAllowedCourseModel>
{
    public int Ukprn { get; set; }
    public string LarsCode { get; set; }
    public PutProviderAllowedCourseModel? Data { get; set; }
    public string PutUrl => $"providers/{Ukprn}/allowed-courses/{LarsCode}";
    public PutProviderAllowedCourseRequest(int ukprn, string larsCode, PutProviderAllowedCourseModel data)
    {
        Ukprn = ukprn;
        LarsCode = larsCode;
        Data = data;
    }
}
