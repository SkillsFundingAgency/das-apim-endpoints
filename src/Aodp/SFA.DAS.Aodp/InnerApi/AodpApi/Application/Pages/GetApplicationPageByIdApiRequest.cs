using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

public class GetApplicationPageByIdApiRequest : IGetApiRequest
{
    public int PageOrder { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }

    public string GetUrl => $"/api/applications/forms/{FormVersionId}/sections/{SectionId}/Pages/{PageOrder}";
}

