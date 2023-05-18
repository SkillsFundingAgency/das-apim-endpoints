namespace SFA.DAS.ApprenticeAan.Application.Services;

public interface IAanHubRestApiClient
{
    Task<RestApiResponse<TResponse>> Get<TResponse>(string url, KeyValuePair<string, string>[] headers);
}