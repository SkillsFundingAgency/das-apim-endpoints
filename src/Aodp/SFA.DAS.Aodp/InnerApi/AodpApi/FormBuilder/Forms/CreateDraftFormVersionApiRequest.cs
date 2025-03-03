using SFA.DAS.SharedOuterApi.Configuration;
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