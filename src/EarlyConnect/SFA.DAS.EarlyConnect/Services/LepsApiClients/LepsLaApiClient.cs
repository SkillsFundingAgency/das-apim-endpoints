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

namespace SFA.DAS.EarlyConnect.Services.LepsApiClients
{
    public class LepsLaApiClient : ILepsLaApiClient<LepsLaApiConfiguration>
    {
        private IInternalApiClient<LepsLaApiConfiguration> _apiClient;
        protected LepsLaApiConfiguration Configuration;
        protected HttpClient HttpClient;
        public LepsLaApiClient(IInternalApiClient<LepsLaApiConfiguration> apiClient, IHttpClientFactory httpClientFactory, LepsLaApiConfiguration apiConfiguration)
        {
            InitializeAsync(apiClient, httpClientFactory, apiConfiguration);
        }
        private async Task<LepsLaApiClient> InitializeAsync(IInternalApiClient<LepsLaApiConfiguration> apiClient, IHttpClientFactory httpClientFactory, LepsLaApiConfiguration apiConfiguration)
        {
            _apiClient = apiClient;
            Configuration = apiConfiguration;

            // Retrieve the client SSL certificate from Azure Key Vault
            var certificateClient = new CertificateClient(new Uri(Configuration.KeyVaultIdentifier), new DefaultAzureCredential());
            var certificateName = Configuration.CertificateName;

            // Retrieve the certificate from Azure Key Vault
            KeyVaultCertificate certificate = await certificateClient.GetCertificateAsync(certificateName);

            // Retrieve the certificate data
            byte[] certificateBytes = certificate.Cer;

            // Create X509Certificate2 object from the certificate data
            var certificateX509 = new X509Certificate2(certificateBytes);

            // Create HttpClientHandler and configure client certificate
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ClientCertificates.Add(certificateX509);
            httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Automatic;

            HttpClient = new HttpClient(httpClientHandler);
            HttpClient.BaseAddress = new Uri(apiConfiguration.Url);

            return this;
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
    }
}