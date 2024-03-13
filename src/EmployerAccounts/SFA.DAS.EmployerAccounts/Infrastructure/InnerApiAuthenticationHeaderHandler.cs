using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Api.Common.Interfaces;

namespace SFA.DAS.EmployerAccounts.Infrastructure
{
    public class InnerApiAuthenticationHeaderHandler : DelegatingHandler
    {
        private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;
        private readonly string _apiIdentifier;

        public InnerApiAuthenticationHeaderHandler(IAzureClientCredentialHelper azureClientCredentialHelper, string apiIdentifier)
        {
            _azureClientCredentialHelper = azureClientCredentialHelper;
            _apiIdentifier = apiIdentifier;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("X-Version", "1.0");
            if (!string.IsNullOrEmpty(_apiIdentifier))
            {
                var accessToken = await _azureClientCredentialHelper.GetAccessTokenAsync(_apiIdentifier);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            Console.WriteLine($"Sending HTTP request: {request.Method} {request.RequestUri}");


            if (request.Headers != null)
            {
                Console.WriteLine("RequestHEaders: ");
                foreach (var header in request.Headers)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }
            }

            if (request.Content != null)
            {
                Console.WriteLine($"Request Content: {await request.Content.ReadAsStringAsync()}");
            }
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}