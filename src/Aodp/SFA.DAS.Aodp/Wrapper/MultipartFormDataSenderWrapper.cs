using Microsoft.AspNetCore.Http;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Text.Json;

namespace SFA.DAS.Aodp.Wrapper;

public class MultipartFormDataSenderWrapper : IMultipartFormDataSenderWrapper
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AodpApiConfiguration _aodpApiConfiguration;
    private static readonly JsonSerializerOptions JsonSerializationOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    public MultipartFormDataSenderWrapper(IHttpClientFactory httpClientFactory,
                                            AodpApiConfiguration aodpApiConfiguration)
    {
        _httpClientFactory = httpClientFactory;
        _aodpApiConfiguration = aodpApiConfiguration;
    }

    public async Task<ApiResponse<TResponse>> PostWithMultipartFormData<TData, TResponse>(IPostApiRequest<TData> request, bool includeResponse = true)
    {
        var requestData = request.Data as IFormFile;
        using var content = new MultipartFormDataContent();
        using var fileStream = requestData?.OpenReadStream();

        var fileContent = new StreamContent(fileStream!);
        fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
        {
            Name = "file",
            FileName = requestData?.FileName
        };

        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(requestData!.ContentType);
        content.Add(fileContent, "file", requestData.FileName);

        var httpClient = _httpClientFactory.CreateClient();
        string url = $"{_aodpApiConfiguration.Url.TrimEnd('/')}/{request.PostUrl.TrimStart('/')}";

        var response = await httpClient.PostAsync(url, content).ConfigureAwait(false);

        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var errorContent = "";
        var responseBody = (TResponse)default!;

        if (IsNot200RangeResponseCode(response.StatusCode))
        {
            errorContent = json;
            HandleException(response, json);
        }
        else if (includeResponse)
        {
            responseBody = JsonSerializer.Deserialize<TResponse>(json, JsonSerializationOptions);
        }

        var postWithResponseCode = new ApiResponse<TResponse>(responseBody!, response.StatusCode, errorContent);

        return postWithResponseCode;
    }

    private static bool IsNot200RangeResponseCode(HttpStatusCode statusCode)
    {
        return !((int)statusCode >= 200 && (int)statusCode <= 299);
    }

    private static string HandleException(HttpResponseMessage response, string json)
    {
        return json;
    }
}
