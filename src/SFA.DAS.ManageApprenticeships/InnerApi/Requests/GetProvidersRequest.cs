using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ManageApprenticeships.InnerApi.Requests
{
    public class GetProvidersRequest : IGetApiRequest
    {
        public string GetUrl => "api/providers";
    }
}