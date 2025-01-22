using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;

public class GetAllFormVersionsApiRequest : IGetApiRequest
{
    public string GetUrl => "/api/forms";
}