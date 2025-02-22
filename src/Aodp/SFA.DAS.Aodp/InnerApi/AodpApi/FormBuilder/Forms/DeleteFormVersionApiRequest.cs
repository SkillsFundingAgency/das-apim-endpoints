using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;

public class DeleteFormVersionApiRequest : IDeleteApiRequest
{
    public readonly Guid FormVersionId;

    public DeleteFormVersionApiRequest(Guid formVersionId)
    {
        FormVersionId = formVersionId;
    }

    public string DeleteUrl => $"/api/forms/{FormVersionId}";
}