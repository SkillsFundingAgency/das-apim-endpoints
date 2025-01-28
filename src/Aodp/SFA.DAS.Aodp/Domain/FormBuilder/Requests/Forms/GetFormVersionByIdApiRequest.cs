using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Domain.FormBuilder.Requests.Forms;

public class GetFormVersionByIdApiRequest : IGetApiRequest
{
    private readonly Guid _formVersionId;
    public GetFormVersionByIdApiRequest(Guid formVersionId)
    {
        _formVersionId = formVersionId;
    }

    public string GetUrl => $"/api/forms/{_formVersionId}";
}