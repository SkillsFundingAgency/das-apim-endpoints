using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.InnerApi.Requests
{
    public class GetEnglishFractionCurrentRequest : IGetApiRequest
    {
        private readonly UriBuilder _baseUri;

        public GetEnglishFractionCurrentRequest(string hashedAccountId, string[] empRefs)
        {
            _baseUri = new UriBuilder
            {
                Path = $"api/accounts/{hashedAccountId}/levy/english-fraction-current"
            };

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
