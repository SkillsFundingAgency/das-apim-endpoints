using SFA.DAS.SharedOuterApi.Interfaces;
namespace SFA.DAS.Aodp.Domain.FormBuilder.Requests.Forms;

public class CreateFormVersionApiRequest : IPostApiRequest
{
    public string PostUrl => "/api/forms";

    public object Data { get; set; }

}