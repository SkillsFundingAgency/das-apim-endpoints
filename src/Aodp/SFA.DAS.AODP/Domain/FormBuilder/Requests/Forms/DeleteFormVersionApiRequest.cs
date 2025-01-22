using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;

public class DeleteFormVersionApiRequest : IDeleteApiRequest
{
    private readonly Guid _formVersionId;

    public DeleteFormVersionApiRequest(Guid formVersionId)
    {
        _formVersionId = formVersionId;
    }

    public string DeleteUrl => $"/api/forms/{_formVersionId}";
}