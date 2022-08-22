using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.InnerApi.Requests
{
    public class GetEnglishFractionCurrentRequest : IGetApiRequest
    {
        private readonly string _hashedAccountId;
        private readonly UriBuilder _baseUri;

        public GetEnglishFractionCurrentRequest(string hashedAccountId, string[] empRefs)
        {
            _hashedAccountId = hashedAccountId;
            
            _baseUri = new UriBuilder();
            _baseUri.Path = $"api/accounts/{_hashedAccountId}/levy/english-fraction-current";

            foreach (string empRef in empRefs)
            {
                if (!string.IsNullOrEmpty(_baseUri.Query))
                    _baseUri.Query += $"&empRefs={empRef}";
                else
                    _baseUri.Query = $"empRefs={empRef}";
            }
        }

        public string GetUrl => _baseUri.Uri.PathAndQuery;
    }
}
