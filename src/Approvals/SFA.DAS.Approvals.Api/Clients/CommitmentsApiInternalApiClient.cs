using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Approvals.ErrorHandling;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using System;
using System.Net;
using System.Net.Http;
using SFA.DAS.Approvals.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.Clients;

public class CommitmentsApiInternalApiClient : InternalApiClient<CommitmentsV2ApiConfiguration>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CommitmentsApiInternalApiClient> _logger;

    public CommitmentsApiInternalApiClient(
        IHttpClientFactory httpClientFactory,
        CommitmentsV2ApiConfiguration apiConfiguration,
        IAzureClientCredentialHelper azureClientCredentialHelper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<CommitmentsApiInternalApiClient> logger) 
        : base(httpClientFactory, apiConfiguration, azureClientCredentialHelper)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    protected override async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
    {
        _logger.LogInformation("=== COMMITMENTS API CLIENT: Starting AddAuthenticationHeader ===");
        _logger.LogInformation("Request URI: {RequestUri}", httpRequestMessage.RequestUri);
        
        // Log all incoming headers for debugging
        if (_httpContextAccessor.HttpContext?.Request.Headers != null)
        {
            _logger.LogInformation("=== INCOMING REQUEST HEADERS ===");
            foreach (var header in _httpContextAccessor.HttpContext.Request.Headers)
            {
                _logger.LogInformation("Header: {HeaderKey} = {HeaderValue}", header.Key, header.Value);
            }
        }
        else
        {
            _logger.LogWarning("HttpContext or Request.Headers is null!");
        }

        // Try to forward the user's token first (for token pass-through)
        var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-Authorization"].FirstOrDefault();
        if (!string.IsNullOrEmpty(authHeader))
        {
            httpRequestMessage.Headers.Add("Authorization", authHeader);
            _logger.LogInformation("X-Forwarded-Authorization header attached to request message: {AuthHeader}", 
                authHeader.Substring(0, Math.Min(50, authHeader.Length)) + "...");
            return;
        }
        else
        {
            _logger.LogInformation("X-Forwarded-Authorization header not found");
        }

        authHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.FirstOrDefault();
        if (!string.IsNullOrEmpty(authHeader))
        {
            httpRequestMessage.Headers.Add("Authorization", authHeader);
            _logger.LogInformation("Authorization header attached to request message: {AuthHeader}", 
                authHeader.Substring(0, Math.Min(50, authHeader.Length)) + "...");
            return;
        }
        else
        {
            _logger.LogInformation("Authorization header not found");
        }

        // Fall back to service-to-service authentication if no user token is available
        _logger.LogInformation("No user token found, falling back to service-to-service authentication.");
        await base.AddAuthenticationHeader(httpRequestMessage);
        
        _logger.LogInformation("=== COMMITMENTS API CLIENT: Completed AddAuthenticationHeader ===");
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