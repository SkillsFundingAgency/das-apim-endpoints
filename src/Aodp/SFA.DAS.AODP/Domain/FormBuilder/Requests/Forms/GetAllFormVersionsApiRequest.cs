using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Domain.FormBuilder.Requests.Forms;

public class GetAllFormVersionsApiRequest : IGetApiRequest
{
    public string GetUrl => "/api/forms";
}