using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;

public class CreateDraftFormVersionApiRequest : IPutApiRequest
{
    public readonly Guid FormId;

    public CreateDraftFormVersionApiRequest(Guid formId)
    {
        FormId = formId;
        Data = new object(); //Unused
    }

    public object Data { get; set; }

    public string PutUrl => $"/api/forms/{FormId}/new-version";
}