using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Approvals.ErrorHandling;
using SFA.DAS.Approvals.Exceptions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.Approvals.Api.Clients;

public class CommitmentsApiInternalApiClient(
    IHttpClientFactory httpClientFactory,
    CommitmentsV2ApiConfiguration apiConfiguration,
    IAzureClientCredentialHelper azureClientCredentialHelper,
    ILogger<CommitmentsApiInternalApiClient> logger)
    : InternalApiClient<CommitmentsV2ApiConfiguration>(httpClientFactory, apiConfiguration, azureClientCredentialHelper)
{

    public override string HandleException(HttpResponseMessage response, string content)
    {
        logger.LogDebug("In exception handling for commitments api internal client");
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
        logger.LogDebug("In CreateApiModelException");
        if (string.IsNullOrWhiteSpace(content))
        {
            logger.LogWarning($"{ httpResponseMessage.RequestMessage.RequestUri} has returned an empty string when an array of error responses was expected.");
        }

        var errors = new DomainApimException(content);
        return errors;
    }

    private Exception CreateBulkUploadApiModelException(HttpResponseMessage httpResponseMessage, string content)
    {
        logger.LogDebug("In CreateBulkUploadApiModelException");
        if (string.IsNullOrWhiteSpace(content))
        {
            logger.LogWarning($"{httpResponseMessage.RequestMessage.RequestUri} has returned an empty string when an array of error responses was expected.");
        }

        var errors = new BulkUploadApimDomainException(content);
        return errors;
    }
}