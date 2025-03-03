using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;

public class GetFormVersionByIdApiRequest : IGetApiRequest
{
    private readonly Guid _formVersionId;
    public GetFormVersionByIdApiRequest(Guid formVersionId)
    {
        _formVersionId = formVersionId;
    }

    public string GetUrl => $"/api/forms/{_formVersionId}";
}