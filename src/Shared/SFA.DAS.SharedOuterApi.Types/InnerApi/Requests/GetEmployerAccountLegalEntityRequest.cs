using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests
{
    public class GetEmployerAccountLegalEntityRequest : IGetApiRequest
    {
        private readonly string _href;

        public GetEmployerAccountLegalEntityRequest(string href)
        {
            _href = href;
        }

        public string GetUrl => _href;
    }
}