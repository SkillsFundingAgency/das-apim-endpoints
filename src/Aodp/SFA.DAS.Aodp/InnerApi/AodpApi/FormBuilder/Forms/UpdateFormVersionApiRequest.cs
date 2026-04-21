using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;

public class UpdateFormVersionApiRequest : IPutApiRequest
{
    public Guid FormVersionId { get; set; }
    public object Data { get; set; }

    public string PutUrl => $"/api/forms/{FormVersionId}";
}