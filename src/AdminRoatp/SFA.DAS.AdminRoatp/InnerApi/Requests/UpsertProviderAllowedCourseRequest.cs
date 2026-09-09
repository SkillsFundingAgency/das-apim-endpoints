using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public class UpsertProviderAllowedCourseRequest : IPostApiRequest
{
    public int Ukprn { get; set; }
    public string LarsCode { get; set; }
    public object Data { get; set; }
    public string PostUrl => $"providers/{Ukprn}/allowed-courses/{LarsCode}";
    public UpsertProviderAllowedCourseRequest(int ukprn, string larsCode, UpsertProviderAllowedCourseModel data)
    {
        Ukprn = ukprn;
        LarsCode = larsCode;
        Data = data;
    }
}
