using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Request
{
    public class GetAllFormsRequest : IGetApiRequest
    {
        public string GetUrl => "/api/forms";
    }
}
