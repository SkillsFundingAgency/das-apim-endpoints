using Microsoft.Extensions.Logging;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Approvals.ErrorHandling;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.Clients
{
    public class CommitmentsApiInternalApiClient : InternalApiClient<CommitmentsV2ApiConfiguration>
    {
        public const string Employer = nameof(Employer);
        public const string Provider = nameof(Provider);

        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;
        private ILogger<CommitmentsApiInternalApiClient> _logger;

        public CommitmentsApiInternalApiClient(IHttpClientFactory httpClientFactory, 
            CommitmentsV2ApiConfiguration apiConfiguration, 
            IAzureClientCredentialHelper azureClientCredentialHelper,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor,
            ILogger<CommitmentsApiInternalApiClient> logger) 
            : base(httpClientFactory, apiConfiguration, azureClientCredentialHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        protected override async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            await base.AddAuthenticationHeader(httpRequestMessage);
            var isEmployer = IsUserInRole(Employer);
            var isProvider = IsUserInRole(Provider);

            if (!isEmployer && !isProvider)
            {
                throw new Exception("User must be either provider or employer");
            }

            httpRequestMessage.Headers.Add("x-party", isEmployer ? Employer : Provider);
        }

        public bool IsUserInRole(string role)
        {
            return _httpContextAccessor.HttpContext.User.IsInRole(role);
        }

        public override string HandleException(HttpResponseMessage response, string content)
        {
            _logger.LogDebug("In exception handling for commitments api internal client");
            if (response.StatusCode == HttpStatusCode.BadRequest && response.GetSubStatusCode() == HttpSubStatusCode.DomainException)
            {
                throw CreateApiModelException(response, content);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest && response.GetSubStatusCode() == HttpSubStatusCode.BulkUploadDomainException)
            {
                throw CreateBulkUploadApiModelException(response, content);
            }
            else
            {
                throw new RestHttpClientException(response, content);
            }
        }
      
        private Exception CreateApiModelException(HttpResponseMessage httpResponseMessage, string content)
        {
            _logger.LogDebug("In CreateApiModelException");
            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogWarning($"{ httpResponseMessage.RequestMessage.RequestUri} has returned an empty string when an array of error responses was expected.");
            }

            var errors = new DomainApimException(content);
            return errors;
        }

        private Exception CreateBulkUploadApiModelException(HttpResponseMessage httpResponseMessage, string content)
        {
            _logger.LogDebug("In CreateBulkUploadApiModelException");
            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogWarning($"{httpResponseMessage.RequestMessage.RequestUri} has returned an empty string when an array of error responses was expected.");
            }

            var errors = new BulkUploadApimDomainException(content);
            return errors;
        }
    }
}
