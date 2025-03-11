using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;

public class GetAllFormVersionsApiRequest : IGetApiRequest
{
    public string GetUrl => "/api/forms";
}