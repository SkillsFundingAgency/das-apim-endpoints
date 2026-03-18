using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

public class GetApplicationFormByIdApiRequest : IGetApiRequest
{
    public Guid FormVersionId { get; set; }

    public string GetUrl => $"/api/applications/forms/{FormVersionId}";
}
