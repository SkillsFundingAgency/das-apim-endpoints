using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
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