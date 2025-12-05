using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;

public class CreateFormVersionApiRequest : IPostApiRequest
{
    public string PostUrl => "/api/forms";

    public required object Data { get; set; }

}