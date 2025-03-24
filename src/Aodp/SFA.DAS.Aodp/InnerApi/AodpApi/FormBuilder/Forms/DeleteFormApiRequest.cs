using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;

public class DeleteFormApiRequest : IDeleteApiRequest
{
    public readonly Guid FormId;

    public DeleteFormApiRequest(Guid formId)
    {
        FormId = formId;
    }

    public string DeleteUrl => $"/api/forms/{FormId}";
}