using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

public class GetApplicationSectionStatusByIdApiRequest : IGetApiRequest
{
    public Guid ApplicationId { get; set; }
    public Guid SectionId { get; set; }
    public Guid FormVersionId { get; set; }

    public string GetUrl => $"/api/applications/{ApplicationId}/forms/{FormVersionId}/sections/{SectionId}";

}
