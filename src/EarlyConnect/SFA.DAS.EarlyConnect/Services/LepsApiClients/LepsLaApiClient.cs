using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using SFA.DAS.SharedOuterApi.Infrastructure;
using Azure.Security.KeyVault.Certificates;
using Azure.Identity;
using SFA.DAS.EarlyConnect.Services.Interfaces;
using SFA.DAS.EarlyConnect.Services.Configuration;
using System.Net.Security;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace SFA.DAS.EarlyConnect.Services.LepsApiClients
{
    public class LepsLaApiClient : ILepsLaApiClient<LepsLaApiConfiguration>
    {
        private IInternalApiClient<LepsLaApiConfiguration> _apiClient;
        protected LepsLaApiConfiguration Configuration;
        protected HttpClient HttpClient;
        private readonly ILogger<LepsLaApiClient> _logger;

        public LepsLaApiClient(IInternalApiClient<LepsLaApiConfiguration> apiClient,
            LepsLaApiConfiguration apiConfiguration, ILogger<LepsLaApiClient> logger)
        {
            _apiClient = apiClient;
            Configuration = apiConfiguration;
            _logger = logger;
        }

        public Task<TResponse> Get<TResponse>(IGetApiRequest request) => _apiClient.Get<TResponse>(request);

        public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request) => _apiClient.GetAll<TResponse>(request);

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request) => _apiClient.GetResponseCode(request);

        public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request) => _apiClient.GetWithResponseCode<TResponse>(request);

        public Task<TResponse> Post<TResponse>(IPostApiRequest request) => _apiClient.Post<TResponse>(request);

        public async Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
        {
            var stringContent = request.Data != null ? new StringContent(JsonSerializer.Serialize(request.Data), Encoding.UTF8, "application/json") : null;

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, request.PostUrl)
            {
                Content = stringContent
            };
            requestMessage.AddVersion(request.Version);

            AddAuthenticationCertificate();

            // Capture start time
            var startTime = DateTime.UtcNow;

            // Log Request Details
            _logger.LogInformation("🔹 [API Request] Sending HTTP {Method} request to: {Url}", requestMessage.Method, request.PostUrl);
            _logger.LogInformation("🔹 [Request Headers] {Headers}", string.Join("; ", requestMessage.Headers.Select(h => $"{h.Key}: {string.Join(",", h.Value)}")));

            if (stringContent != null)
            {
                var requestBody = await stringContent.ReadAsStringAsync();
                _logger.LogInformation("🔹 [Request Body] {Body}", requestBody);
            }

            HttpResponseMessage response;
            try
            {
                response = await HttpClient.SendAsync(requestMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ [API Call Failed] Exception while making request to {Url}: {Error}", request.PostUrl, ex.Message);

                // Log inner exceptions recursively
                Exception innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    _logger.LogError("❌ [Inner Exception] {Error}", innerEx.Message);
                    innerEx = innerEx.InnerException;
                }

                throw;
            }


            var endTime = DateTime.UtcNow;
            var executionTime = (endTime - startTime).TotalMilliseconds;

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Log Response Details
            _logger.LogInformation("🔹 [API Response] Status Code: {StatusCode}, Execution Time: {ExecutionTime} ms", response.StatusCode, executionTime);
            _logger.LogInformation("🔹 [Response Headers] {Headers}", string.Join("; ", response.Headers.Select(h => $"{h.Key}: {string.Join(",", h.Value)}")));
            _logger.LogInformation("🔹 [Response Body] {Body}", json);

            var errorContent = "";
            var responseBody = (TResponse)default;

            if (IsNot200RangeResponseCode(response.StatusCode))
            {
                errorContent = json;
                HandleException(response, json);
            }
            else if (includeResponse)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                responseBody = JsonSerializer.Deserialize<TResponse>(json, options);
            }

            return new ApiResponse<TResponse>(responseBody, response.StatusCode, errorContent);
        }


        public Task Post<TData>(IPostApiRequest<TData> request) => _apiClient.Post(request);

        public Task Delete(IDeleteApiRequest request) => _apiClient.Delete(request);

        public Task Patch<TData>(IPatchApiRequest<TData> request) => _apiClient.Patch(request);

        public Task Put(IPutApiRequest request) => _apiClient.Put(request);

        public Task Put<TData>(IPutApiRequest<TData> request) => _apiClient.Put(request);

        public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request) => throw new NotImplementedException();

        public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request) => throw new NotImplementedException();

        public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request) => throw new NotImplementedException();

        private static bool IsNot200RangeResponseCode(HttpStatusCode statusCode) => !((int)statusCode >= 200 && (int)statusCode <= 299);

        public virtual string HandleException(HttpResponseMessage response, string json) => json;

        private void AddAuthenticationCertificate()
        {
            try
            {
                var defaultAzureCredentialOptions = new DefaultAzureCredentialOptions();
                var certificateClient = new CertificateClient(new Uri(Configuration.KeyVaultIdentifier), new DefaultAzureCredential(defaultAzureCredentialOptions));
                var certificate = certificateClient.DownloadCertificate(Configuration.CertificateName);

                if (certificate == null || certificate.Value == null)
                {
                    throw new Exception("❌ Certificate was not properly returned from the Key Vault.");
                }

                if (!certificate.Value.HasPrivateKey)
                {
                    throw new Exception("❌ Certificate has no private key.");
                }

                var httpClientHandler = new HttpClientHandler
                {
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                    ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                    {
                        if (sslPolicyErrors != SslPolicyErrors.None)
                        {
                            _logger.LogError("❌ [TLS Error] SSL Policy Error: {Error}", sslPolicyErrors);
                            return false;
                        }

                        _logger.LogInformation("🔹 [TLS Handshake] Successful with Server Certificate: {Subject}", cert.Subject);
                        _logger.LogInformation("🔹 [TLS Version] {TlsVersion}", message.Version);

                        return true;
                    }
                };

                //try
                //{
                //    using (var tcpClient = new System.Net.Sockets.TcpClient("dfedata-uat.lancashire.gov.uk", 443))
                //    using (var sslStream = new SslStream(tcpClient.GetStream(), false, (sender, certificate, chain, errors) => true))
                //    {
                //        sslStream.AuthenticateAsClient("dfedata-uat.lancashire.gov.uk");
                //        _logger.LogInformation("🔹 [Actual TLS Version] {TlsVersion}", sslStream.SslProtocol);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    _logger.LogError("❌ [TLS Detection Error] Failed to determine TLS version: {Error}", ex.Message);
                //}

                // Log Certificate Information
                _logger.LogInformation("🔹 [Certificate Info] Using Certificate for Authentication:");
                _logger.LogInformation("   🔸 Subject: {Subject}", certificate.Value.Subject);
                _logger.LogInformation("   🔸 Issuer: {Issuer}", certificate.Value.Issuer);
                _logger.LogInformation("   🔸 Valid From: {NotBefore}", certificate.Value.NotBefore);
                _logger.LogInformation("   🔸 Expiry Date: {NotAfter}", certificate.Value.NotAfter);
                _logger.LogInformation("   🔸 Has Private Key: {HasPrivateKey}", certificate.Value.HasPrivateKey);

                httpClientHandler.ClientCertificates.Add(certificate);

                HttpClient = new HttpClient(httpClientHandler)
                {
                    BaseAddress = new Uri(Configuration.Url)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("❌ [Certificate Error] Failed to load certificate: {Error}", ex.Message);
                throw;
            }
        }


        public Task<ApiResponse<TResponse>> PatchWithResponseCode<TData, TResponse>(IPatchApiRequest<TData> request, bool includeResponse = true) => throw new NotImplementedException();
    }
}
