using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Sections;

public class GetAllSectionsApiRequest : IGetApiRequest
{
    public Guid FormVersionId { get; set; }


    public string GetUrl => $"/api/forms/{FormVersionId}/sections";
}