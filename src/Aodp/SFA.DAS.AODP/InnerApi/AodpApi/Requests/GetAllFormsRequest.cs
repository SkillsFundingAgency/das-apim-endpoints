using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.InnerApi.AodpApi.Request
{
    public class GetAllFormsRequest : IGetApiRequest
    {
        public string GetUrl => "/api/forms";
    }
}
