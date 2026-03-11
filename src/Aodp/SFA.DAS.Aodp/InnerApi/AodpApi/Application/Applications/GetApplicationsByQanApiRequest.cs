using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Application.Applications;

public class GetApplicationsByQanApiRequest : IGetApiRequest
{
    public string? Qan { get; set; }
    public string GetUrl => $"/api/applications/qualifications/{Qan}";
}
