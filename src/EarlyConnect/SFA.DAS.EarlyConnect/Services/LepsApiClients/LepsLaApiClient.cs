﻿using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Runtime.InteropServices;
//using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using SFA.DAS.SharedOuterApi.Infrastructure;
using Azure.Security.KeyVault.Certificates;
//using Azure.Identity;
using SFA.DAS.EarlyConnect.Services.Interfaces;
using SFA.DAS.EarlyConnect.Services.Configuration;
//using System.Net.Http;
//using Azure.Security.KeyVault.Secrets;
//using System.Security.Cryptography;
//using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace SFA.DAS.EarlyConnect.Services.LepsApiClients
{
    public class LepsLaApiClient : ILepsLaApiClient<LepsLaApiConfiguration>
    {
        private IInternalApiClient<LepsLaApiConfiguration> _apiClient;
        protected LepsLaApiConfiguration Configuration;
        protected HttpClient HttpClient;
        public LepsLaApiClient(IInternalApiClient<LepsLaApiConfiguration> apiClient,
            IHttpClientFactory httpClientFactory,
            LepsLaApiConfiguration apiConfiguration)
        {
            HttpClient = httpClientFactory.CreateClient("SecureClient");
            HttpClient.BaseAddress = new Uri(apiConfiguration.Url);
            Configuration = apiConfiguration;
        }

        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return _apiClient.Get<TResponse>(request);
        }

        public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            return _apiClient.GetAll<TResponse>(request);
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            return _apiClient.GetResponseCode(request);
        }

        public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            return _apiClient.GetWithResponseCode<TResponse>(request);
        }

        public Task<TResponse> Post<TResponse>(IPostApiRequest request)
        {
            return _apiClient.Post<TResponse>(request);
        }

        public async Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
        {
            var stringContent = request.Data != null ? new StringContent(JsonSerializer.Serialize(request.Data), Encoding.UTF8, "application/json") : null;

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, request.PostUrl);
            requestMessage.AddVersion(request.Version);
            requestMessage.Content = stringContent;

            //AddAuthenticationCertificate();

            var response = await HttpClient.SendAsync(requestMessage).ConfigureAwait(false);

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var errorContent = "";
            var responseBody = (TResponse)default;

            if (IsNot200RangeResponseCode(response.StatusCode))
            {
                errorContent = json;
                HandleException(response, json);
            }
            else if (includeResponse)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                responseBody = JsonSerializer.Deserialize<TResponse>(json, options);
            }

            var postWithResponseCode = new ApiResponse<TResponse>(responseBody, response.StatusCode, errorContent);

            return postWithResponseCode;
        }

        public Task Post<TData>(IPostApiRequest<TData> request)
        {
            return _apiClient.Post(request);
        }

        public Task Delete(IDeleteApiRequest request)
        {
            return _apiClient.Delete(request);
        }

        public Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            return _apiClient.Patch(request);
        }

        public Task Put(IPutApiRequest request)
        {
            return _apiClient.Put(request);
        }

        public Task Put<TData>(IPutApiRequest<TData> request)
        {
            return _apiClient.Put(request);
        }
        public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
        {
            throw new NotImplementedException();
        }
        private static bool IsNot200RangeResponseCode(HttpStatusCode statusCode)
        {
            return !((int)statusCode >= 200 && (int)statusCode <= 299);
        }
        public virtual string HandleException(HttpResponseMessage response, string json)
        {
            return json;
        }

        //private void AddAuthenticationCertificate()
        //{
        //    try
        //    {
        //        var defaultAzureCredentialOptions = new DefaultAzureCredentialOptions();
        //        var certificateClient = new CertificateClient(vaultUri: new Uri(Configuration.KeyVaultIdentifier), credential: new DefaultAzureCredential(defaultAzureCredentialOptions));
        //        var certificate = certificateClient.DownloadCertificate(Configuration.CertificateName);

        //        if (certificate == null || certificate.Value == null)
        //        {
        //            throw new Exception("Certificate was not properly returned from the Key Vault.");
        //        }
        //        var httpClientHandler = new HttpClientHandler();
        //        httpClientHandler.ClientCertificates.Add(certificate);

        //        HttpClient = new HttpClient(httpClientHandler);
        //        HttpClient.BaseAddress = new Uri(Configuration.Url);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("An error occurred while reading certificate: " + ex.Message);
        //        throw;
        //    }
        //}

        //private void AddAuthenticationCertificate()
        //{
        //    try
        //    {
        //        // Authenticate with Azure Key Vault
        //        var credential = new DefaultAzureCredential();
        //        var secretClient = new SecretClient(new Uri(Configuration.KeyVaultIdentifier), credential);

        //        // Retrieve the certificate as a secret (Base64 encoded PFX)
        //        KeyVaultSecret certificateSecret = secretClient.GetSecret(Configuration.CertificateName);
        //        byte[] certificateBytes = Convert.FromBase64String(certificateSecret.Value);

        //        // Load certificate with private key
        //        X509Certificate2 certificate = new X509Certificate2(certificateBytes, (string)null,
        //            X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);

        //        if (!certificate.HasPrivateKey)
        //        {
        //            throw new Exception("Certificate does not contain a private key.");
        //        }

        //        // Extract Private Key (RSA or ECDSA)
        //        RSA rsaKey = certificate.GetRSAPrivateKey();
        //        ECDsa ecdsaKey = certificate.GetECDsaPrivateKey();

        //        if (rsaKey == null && ecdsaKey == null)
        //        {
        //            throw new Exception("❌ No valid private key found in the certificate.");
        //        };
           

        //        // Add certificate to HttpClient
        //        var httpClientHandler = new HttpClientHandler();
        //        httpClientHandler.ClientCertificates.Add(certificate);

        //        HttpClient = new HttpClient(httpClientHandler);
        //        HttpClient.BaseAddress = new Uri(Configuration.Url);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("❌ Error loading client certificate: " + ex.Message);
        //        throw;
        //    }
        //}
        public Task<ApiResponse<TResponse>> PatchWithResponseCode<TData, TResponse>(IPatchApiRequest<TData> request, bool includeResponse = true)
        {
            throw new NotImplementedException();
        }
    }
}