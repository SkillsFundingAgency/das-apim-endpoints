using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;

public class GetAllFormVersionsApiRequest : IGetApiRequest
{
    public string GetUrl => "/api/forms";
}