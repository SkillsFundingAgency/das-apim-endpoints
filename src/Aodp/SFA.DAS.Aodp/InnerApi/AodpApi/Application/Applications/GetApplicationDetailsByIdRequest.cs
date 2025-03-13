using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Application.Applications;

public class GetApplicationDetailsByIdRequest : IGetApiRequest
{
    public Guid ApplicationId { get; set; }
    public string GetUrl => $"/api/applications/{ApplicationId}/details";
}
