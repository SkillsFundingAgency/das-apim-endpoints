﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.Clients;

/// <summary>
/// To bypass the MI authentication. 
/// Is used only for local development.
/// </summary>
public class LocalCommitmentsApiInternalApiClient(
    IHttpClientFactory httpClientFactory,
    CommitmentsV2ApiConfiguration apiConfiguration,
    IAzureClientCredentialHelper azureClientCredentialHelper,
    IHttpContextAccessor httpContextAccessor,
    ILogger<CommitmentsApiInternalApiClient> logger)
    : CommitmentsApiInternalApiClient(httpClientFactory, apiConfiguration, azureClientCredentialHelper,
        httpContextAccessor, logger)
{
    protected override Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
    {
        // Defaulting to Provider for now. As this change was done for Bulk upload
        // Will need to be looked at when employer starts calling APIM
        var byteArray = Encoding.ASCII.GetBytes($"provider:password1234");
        HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        return Task.FromResult(0);
    }
}