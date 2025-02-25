using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Application.Form;

public class GetApplicationFormPreviewApiRequest : IGetApiRequest
{
    public Guid ApplicationId { get; set; }

    public GetApplicationFormPreviewApiRequest(Guid applicationId)
    {
        ApplicationId = applicationId;
    }

    public string GetUrl => $"/api/applications/{ApplicationId}/form-preview";
}
