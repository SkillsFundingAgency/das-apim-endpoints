using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Sections;

public class UpdateSectionApiRequest : IPutApiRequest
{
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }


    public string PutUrl => $"/api/forms/{FormVersionId}/sections/{SectionId}";

    public object Data { get; set; }

}