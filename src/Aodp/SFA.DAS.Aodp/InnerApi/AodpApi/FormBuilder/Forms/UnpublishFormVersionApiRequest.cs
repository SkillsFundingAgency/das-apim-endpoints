using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;

public class MoveFormDownApiRequest : IPutApiRequest
{
    public readonly Guid FormVersionId;

    public MoveFormDownApiRequest(Guid formVersionId)
    {
        FormVersionId = formVersionId;
        Data = new object(); //Unused
    }

    public object Data { get; set; }

    public string PutUrl => $"/api/forms/{FormVersionId}/MoveDown";
}