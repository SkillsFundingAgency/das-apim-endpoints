using SFA.DAS.Apim.Shared.Interfaces;
namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;

public class CreateFormVersionApiRequest : IPostApiRequest
{
    public string PostUrl => "/api/forms";

    public object Data { get; set; }

}